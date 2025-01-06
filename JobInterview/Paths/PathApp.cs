namespace ScreenshotService
{
    public abstract class PathApp
    {
        public static string BaseDirectory
        {
            get { 
                

                return Path.Combine(AppContext.BaseDirectory, "..", "Data"); 
            
            }
        }
    }
}
