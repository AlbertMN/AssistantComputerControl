using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistantComputerControl {
    public partial class UserFeedback : Form {
        public UserFeedback() {
            InitializeComponent();

            Text = Translator.__("window_name", "user_feedback_window");
        }
    }
}
