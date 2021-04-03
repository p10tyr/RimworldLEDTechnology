namespace ppumkin.LEDTechnology
{
    public static class Log
    {
        public static bool IsTurnedOn = false;
        public static int LogEntries;

        public static void Safe(string message, bool trace = false)
        {
            if (!IsTurnedOn)
            {
                return;
            }

            LogEntries++;

            if (LogEntries == 250)
            {
                Verse.Log.Message(
                    "Logs are getting flooded. Probably a serious error occured. Muting logs to prevent performance issues");
            }

            if (LogEntries < 250)
            {
                Verse.Log.Message(message);
            }
        }
    }
}