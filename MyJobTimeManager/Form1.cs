using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyJobTimeManager
{
    public partial class Form1 : Form
    {
        private static readonly int Work_Time_Count_ = 30 * 60;
        private static readonly int Rest_Time_Count_ = 5 * 60;
        //private static readonly int Work_Time_Count_ = 5;
        //private static readonly int Rest_Time_Count_ = 3;
        private static readonly int Interval = 1000;
        private static readonly int Second_Interval = Interval / 1000;
        private TimeSpan TS_Work_Time_Count_;
        private TimeSpan TS_Rest_Time_Count_;

        private int second_current_type_count_;

        private bool is_rest_time = false;
        private FormWindowState current_form_state_;

        private Timer timer_;
        public Form1()
        {
            InitializeComponent();
            Initialize();

            this.notifyIcon1.Icon = Properties.Resources.Icon1;
            this.WindowState = FormWindowState.Normal;
        }

        private void Initialize()
        {
            TS_Work_Time_Count_ = TimeSpan.FromSeconds(Work_Time_Count_);
            TS_Rest_Time_Count_ = TimeSpan.FromSeconds(Rest_Time_Count_);

            SetTimeCountLabel(TS_Work_Time_Count_);
            label3.Hide();

            timer_ = new Timer() { Interval = 1000 };

            timer_.Tick += (obj, args) =>
            {
                second_current_type_count_ = second_current_type_count_ - Second_Interval;
                TimeSpan remaining = TimeSpan.FromSeconds(second_current_type_count_);

                if (remaining.Seconds > Work_Time_Count_)
                {
                    //error
                    new Exception("error");
                }

                SetTimeCountLabel(remaining);
                
                if (remaining.CompareTo(TimeSpan.Zero) < 1)
                {
                    if (is_rest_time)
                    {
                        ResetWorkTime();
                    }
                    else
                    {
                        second_current_type_count_ = Rest_Time_Count_;

                        SetTimeCountLabel(TS_Rest_Time_Count_);

                        label3.Show();

                        if (WindowState.Equals(FormWindowState.Minimized))
                        {
                            ShowThisForm();
                        }

                        this.WindowState = FormWindowState.Maximized;
                        this.FormBorderStyle = FormBorderStyle.None;
                        this.TopMost = true;
                    }

                    is_rest_time = !is_rest_time;

                }
            };
        }

        private void ResetWorkTime()
        {
            second_current_type_count_ = Work_Time_Count_;

            label3.Hide();

            label2.Text = TS_Work_Time_Count_.ToString("mm\\:ss");

            if (!timer_.Enabled) { timer_.Enabled = true; }
            button1.Text = "Stop";

            if (current_form_state_.Equals(FormWindowState.Minimized))
            {
                HideThisForm();
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = false;
            }
        }

        private void ResetRestTime()
        {

        }

        private void SetTimeCountLabel(TimeSpan time_span)
        {
            label2.Text = time_span.ToString("mm\\:ss");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer_ != null)
            {
                if (timer_.Enabled)
                {
                    timer_.Stop();
                    button1.Text = "Start";

                }else
                {
                    ResetWorkTime();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
             if (WindowState.Equals(FormWindowState.Minimized))
             {
                 HideThisForm();
                 current_form_state_ = WindowState;
             }
             else if(WindowState.Equals(FormWindowState.Normal))
             {
                 this.FormBorderStyle = FormBorderStyle.Sizable;
                 current_form_state_ = WindowState;
             }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowThisForm();
        }

        private void ShowThisForm()
        {
            this.Show();
            notifyIcon1.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void HideThisForm()
        {
            this.Hide();
            notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
