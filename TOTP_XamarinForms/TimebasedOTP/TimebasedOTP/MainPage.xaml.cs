using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OtpNet;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Essentials;
using TimebasedOTP.Services;

namespace TimebasedOTP
{
    public partial class MainPage : ContentPage
    {
        private string generatedOTP;
        public MainPage()
        {
            InitializeComponent();
           
        }

        private async void VerifyOTP_Clicked(object sender, EventArgs e)
        {
            // Get user input OTP from Entry field
            string userEnteredOTP = userInput.Text;

            // Verify the user entered OTP
            if (userEnteredOTP == generatedOTP)
            {
                // OTP is valid
               // await Navigation.PushAsync(new HomePage());
                // Perform the desired action, e.g., allow access or proceed to the next step
            }
            else
            {
                // OTP is invalid
                // Display an error message or take appropriate action
                await DisplayAlert("Error", "Invalid OTP", "OK");
            }

            /*var base32Secret = "JBSWY3DPEHPK3PXP";
            var otp = OtpGenerator.GenerateOtp(base32Secret);
            Console.WriteLine($"New OTP: {otp}");

            var userEnteredOtp = userInput.Text;
            var isOtpValid = OtpGenerator.VerifyOtp(base32Secret, userEnteredOtp);
            Console.WriteLine($"Is OTP valid? {isOtpValid}");*/

        }

        private void SendOTP_Clicked(object sender, EventArgs e)
        {
            string secretKey = "JBSWY3DPEHPK3PXP";

            // Convert secret key to byte array
            byte[] secretKeyBytes = Base32Encoding.ToBytes(secretKey);

            // Create a TOTP instance for generating OTP
            var totp = new Totp(secretKeyBytes);

            // Generate the OTP based on the current time
            generatedOTP = totp.ComputeTotp();

            string recipientEmail = "";

            // Replace with the secret key associated with the user's account

            // Send the OTP to the recipient email
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            MailMessage message = new MailMessage();
            message.From = new MailAddress("senderMail@gmail.com");
            message.To.Add(recipientEmail); // Recipient email address
            message.Subject = "OTP Verification";
            message.Body = "Your OTP is: " + generatedOTP;
            message.Priority = MailPriority.High;
            //message.Bcc.Add("bcc@example.com"); // Add BCC email address
            message.CC.Add("cc@example.com"); // Add CC email address

            SmtpServer.Port = 587; // Use the appropriate port for your email provider
            SmtpServer.Credentials = new System.Net.NetworkCredential("senderMail@gmail.com", "Apppassword");
            SmtpServer.EnableSsl = true;
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            SmtpServer.UseDefaultCredentials = false;


            try
            {
                SmtpServer.Send(message);

                DisplayAlert("Success", "OTP sent to your email", "OK");
                verifyButton.IsEnabled = true;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}", ex.ToString());
                // Handle exceptions while sending email
                DisplayAlert("Error", "Failed to send OTP to email: " + ex.Message, "OK");
            }
        }
    }
}
