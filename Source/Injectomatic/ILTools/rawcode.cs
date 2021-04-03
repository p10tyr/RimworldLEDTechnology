using System;
using System.Collections.Generic;
using System.Reflection;

namespace rawcode.ILTools
{
    public static class Rawcode
    {
        private static string nothing =
            "I spy with my littly ILSpy -> Deliberalty changed names here to prevent clashing with CCL";

        private static readonly List<string> originals = new List<string>();
        private static readonly List<string> injected = new List<string>();

        /**
         * This is a basic first implementation of the IL method jump injection  made possible by RawCode's work;
         * https://ludeon.com/forums/index.php?topic=17143.0
         * * 
         * Community Core Library implementaion for 64bit patching
         * *
         */
        public static unsafe bool TryInjectoFromTo(MethodInfo source, MethodInfo destination)
        {
            // error out on null arguments
            if (source == null)
            {
                throw new ArgumentNullException("Source is NULL");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("Destination is NULL");
            }

            //Log.Message("Source and destination are not null trying to inject jump. ");

            // keep track of detours and spit out some messaging
            var sourceString = source.DeclaringType?.FullName + "." + source.Name + " @ 0x" +
                               source.MethodHandle.GetFunctionPointer().ToString("X" + (IntPtr.Size * 2));
            //Log.Message("Source: " + sourceString);

            var destinationString = destination.DeclaringType?.FullName + "." + destination.Name + " @ 0x" +
                                    destination.MethodHandle.GetFunctionPointer().ToString("X" + (IntPtr.Size * 2));
            //Log.Message("Destination: " + destinationString);


            originals.Add(sourceString);
            injected.Add(destinationString);

            if (IntPtr.Size == sizeof(long))
            {
                // 64-bit systems use 64-bit absolute address and jumps
                // 12 byte destructive

                // Get function pointers
                var Source_Base = source.MethodHandle.GetFunctionPointer().ToInt64();
                var Destination_Base = destination.MethodHandle.GetFunctionPointer().ToInt64();

                // Native source address
                var Pointer_Raw_Source = (byte*) Source_Base;

                // Pointer to insert jump address into native code
                var Pointer_Raw_Address = (long*) (Pointer_Raw_Source + 0x02);

                // Insert 64-bit absolute jump into native code (address in rax)
                // mov rax, immediate64
                // jmp [rax]
                *(Pointer_Raw_Source + 0x00) = 0x48;
                *(Pointer_Raw_Source + 0x01) = 0xB8;
                *Pointer_Raw_Address =
                    Destination_Base; // ( Pointer_Raw_Source + 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 )
                *(Pointer_Raw_Source + 0x0A) = 0xFF;
                *(Pointer_Raw_Source + 0x0B) = 0xE0;

                //Log.Message("64bit Jump - Success");
            }
            else
            {
                // 32-bit systems use 32-bit relative offset and jump
                // 5 byte destructive

                // Get function pointers
                var Source_Base = source.MethodHandle.GetFunctionPointer().ToInt32();
                var Destination_Base = destination.MethodHandle.GetFunctionPointer().ToInt32();

                // Native source address
                var Pointer_Raw_Source = (byte*) Source_Base;

                // Pointer to insert jump address into native code
                var Pointer_Raw_Address = (int*) (Pointer_Raw_Source + 1);

                // Jump offset (less instruction size)
                var offset = Destination_Base - Source_Base - 5;

                // Insert 32-bit relative jump into native code
                *Pointer_Raw_Source = 0xE9;
                *Pointer_Raw_Address = offset;

                //Log.Message("32bit Jump - Success");
            }

            return true;
        }
    }
}