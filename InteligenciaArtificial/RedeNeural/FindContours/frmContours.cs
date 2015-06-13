using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 * Developed by : Lasitha Ishan Petthawadu
 * Email        : petthawadu3@gmail.com
 * Description  : http://wp.me/p429SV-8G
 * Date : 12/06/2014
 */
namespace FindContours
{
    /// <summary>
    /// Main form to interact with EmguCV for contour processing.
    /// </summary>
    public partial class FrmContours : Form
    {
        Emgu.CV.Capture c;
        FindContours processor = new FindContours();
        Bitmap colorImage;

        /// <summary>
        /// Initializes the initial tracker value to th tracker label.
        /// </summary>
        public FrmContours()
        {
            InitializeComponent();
            lblThresholdValue.Text = trackbarThreshold.Value.ToString();
        }

        /// <summary>
        /// Updates the threshold label when the tracker changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackbarThreshold_Scroll(object sender, EventArgs e)
        {
            lblThresholdValue.Text = trackbarThreshold.Value.ToString();
        }

        /// <summary>
        /// Starts the camera capture.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (c == null)
            {
                c = new Emgu.CV.Capture();
            }
            CameraStreamCapture.Enabled = true;

        }

        /// <summary>
        /// Stops the Camera capture.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopCapture_Click(object sender, EventArgs e)
        {
            CameraStreamCapture.Enabled = false;
            if (c != null)
            {
                c.Dispose();
                c = null;
            }

        }
        /// <summary>
        /// Start capturing from the camera stream.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraStreamCapture_Tick(object sender, EventArgs e)
        {
            colorImage = c.QueryFrame().ToBitmap();
            Bitmap color;
            Bitmap gray;
            processor.IdentifyContours(colorImage, trackbarThreshold.Value, chkBoxInvert.Checked, out gray, out color);
            pictBoxColor.Image = color;
            pictBoxGray.Image = gray;
        }

        /// <summary>
        /// Stop the camera and release resources when the form is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmContours_FormClosing(object sender, FormClosingEventArgs e)
        {
            CameraStreamCapture.Enabled = false;
            c = null;
        }
    }
}
