namespace ppumkin.LEDTechnology
{
    public static class Log
    {
        public static int LogEntries = 0;

        public static void Safe(string message, bool trace = false)
        {
            LogEntries++;

            if (LogEntries == 50)
            {
                Verse.Log.Message("Logs are getting flooded. Probably a serious error occured. Muting logs to prevent performance issues");
            }

            if (LogEntries < 50)
                Verse.Log.Message(message);

        }

    }
}
