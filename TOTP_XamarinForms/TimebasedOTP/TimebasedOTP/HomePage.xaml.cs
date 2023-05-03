using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimebasedOTP.Services;

namespace TimebasedOTP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly string _base32Secret = "JBSWY3DPEHPK3PXP";
        private string _otp;
        private DateTime _expiryTime;
        public HomePage()
        {
            InitializeComponent();

            // Set the initial values of CodeDigits and IntervalSeconds based on user input
            digitsLabel.Text = Convert.ToString(OtpGenerator.CodeDigits);
            tokenLabel.Text = Convert.ToString(OtpGenerator.IntervalSeconds);

            // Set the expiry time to the end of the current interval
            _expiryTime = DateTime.UtcNow.AddSeconds(OtpGenerator.IntervalSeconds - DateTime.UtcNow.Second % OtpGenerator.IntervalSeconds);
            
            // Start the timer to update the OTP every interval
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
 		OtpGenerator.CodeDigits = Convert.ToInt32(digitsLabel.Text);
                OtpGenerator.IntervalSeconds = Convert.ToInt32(tokenLabel.Text);
                UpdateProgressBar();
                // Check if the current time has exceeded the expiry time
                if (DateTime.UtcNow >= _expiryTime)
                {
                   
                    // Generate a new OTP and set the expiry time to the end of the new interval
                    _otp = OtpGenerator.GenerateOtp(_base32Secret);
                    _expiryTime = DateTime.UtcNow.AddSeconds(OtpGenerator.IntervalSeconds - DateTime.UtcNow.Second % OtpGenerator.IntervalSeconds);

                    // Update the OTP label and the timestamp and digits labels
                    otpLabel.Text = _otp;
                   
                    //digitsLabel.Text = OtpGenerator.CodeDigits.ToString();
                }

                return true;
            });
        }
        private void UpdateProgressBar()
        {
            // Calculate the percentage of time remaining
            var secondsRemaining = (_expiryTime - DateTime.UtcNow).TotalSeconds;
            var percentageRemaining = secondsRemaining / OtpGenerator.IntervalSeconds;

            // Update the progress bar and the label
            progressBar.Progress = percentageRemaining;
            secondsLabel.Text = $"Updating in {secondsRemaining.ToString("0")} sec";
        }
        private void VerifyOTP_Clicked(object sender, EventArgs e)
        {
            var base32Secret = "JBSWY3DPEHPK3PXP";
            var otp = OtpGenerator.GenerateOtp(base32Secret);
            Console.WriteLine($"New OTP: {otp}");

            var userEnteredOtp = "";
            var isOtpValid = OtpGenerator.VerifyOtp(base32Secret, userEnteredOtp, out long _expiryTime);
            Console.WriteLine($"Is OTP valid? {isOtpValid}");

        }
    }
}