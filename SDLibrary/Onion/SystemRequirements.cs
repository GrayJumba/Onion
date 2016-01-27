using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.SystemR
{
    public class SystemRequirements
    {
        public static bool confirmDotNet45Installed()
        {
            try
            {
                using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
                {
                    int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                    if (releaseKey >= 378389)
                        return true;
                    else
                    {
                        ErrorHandler.Errors.displayError("You are missing .Net version 4.5 which is required for this application to run.Please download and install it from this link: http://www.microsoft.com/en-sg/download/confirmation.aspx?id=30653", ErrorHandler.ErrorCode.DotNet45Missing, ErrorHandler.ErrorAction.Exit, new Exception());
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("Error checking the dot net Version in your computer.", ErrorHandler.ErrorCode.DotNet45Missing, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }

        }
        private static string CheckFor45DotVersion(int releaseKey)
        {
            if (releaseKey >= 393273)
            {
                return "4.6 RC or later";
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2 or later";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1 or later";
            }
            if ((releaseKey >= 378389))
            {
                return "4.5 or later";
            }
            // This line should never execute. A non-null release key should mean 
            // that 4.5 or later is installed. 

            return "No 4.5 or later version detected";
        }

    }
}
