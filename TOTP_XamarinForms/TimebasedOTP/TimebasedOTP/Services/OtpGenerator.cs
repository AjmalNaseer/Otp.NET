using Android.Util;
using OtpNet;
using System;

namespace TimebasedOTP.Services
{
    public static class OtpGenerator
    {
        private static int _codeDigits = 6;
        public static int CodeDigits
        {
            get { return _codeDigits; }
            set { _codeDigits = value; }
        }

        private static int _intervalSeconds = 30;
        public static int IntervalSeconds
        {
            get { return _intervalSeconds; }
            set { _intervalSeconds = value; }
        }
        public static string GenerateOtp(string base32Secret)
        {
            try
            {
                var totp = new Totp(Base32Encoding.ToBytes(base32Secret), step: IntervalSeconds);
                return totp.ComputeTotp().PadLeft(CodeDigits, '0');
            }
            
            catch (Exception ex)
            {
                Log.Error(ex.ToString(), "Error generating OTP");
                throw;
            }
        }

        public static bool VerifyOtp(string base32Secret, string userCode, out long timeStepMatched)
        {
            try
            {
                var totp = new Totp(Base32Encoding.ToBytes(base32Secret), step: IntervalSeconds);
                return totp.VerifyTotp(userCode, out timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay);
            }
            
            catch (Exception ex)
            {
               
                Log.Error(ex.ToString(), "Error verifying OTP");
                throw;
            }
        }
    }
}
