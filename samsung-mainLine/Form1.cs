namespace samsung_mainLine
{
    public partial class Form1 : Form
    {
        public List<AGVCallingModel> AGVData = new();
        public List<AGVErrorModel> AGVError = new();

        public Form1()
        {
            InitializeComponent();
            callAPI();
        }

        private void callAPI()
        {
            List<AGVCallingModel> showData = new();
            List<AGVErrorModel> errorData = new();

            AGVCallingModel temp = new("1", "AGV-1", "Station 3", "Finish");
            AGVCallingModel temp2 = new("2", "AGV-2", "Station 7", "Running");
            AGVCallingModel temp3 = new("3", "AGV-1", "Station 3", "Finish");
            AGVCallingModel temp4 = new("4", "AGV-2", "Station 7", "Running");
            AGVCallingModel temp5 = new("5", "AGV-1", "Station 3", "Finish");
            AGVCallingModel temp6 = new("6", "AGV-2", "Station 7", "Running");
            AGVCallingModel temp7 = new("7", "AGV-1", "Station 3", "Finish");
            AGVCallingModel temp8 = new("8", "AGV-2", "Station 7", "Running");
            AGVCallingModel temp9 = new("9", "AGV-2", "Station 9", "Running");
            AGVCallingModel temp10 = new("10", "AGV-2", "Station 9", "Running");
            showData.Add(temp);
            showData.Add(temp2);
            showData.Add(temp3);
            showData.Add(temp4);
            showData.Add(temp5);
            showData.Add(temp6);
            showData.Add(temp7);
            showData.Add(temp8);
            showData.Add(temp9);
            showData.Add(temp10);
            showData.Add(temp);
            showData.Add(temp2);
            showData.Add(temp3);
            showData.Add(temp4);
            showData.Add(temp5);
            showData.Add(temp6);

            AGVErrorModel err = new("1", "AGV-1", "EMC STOP", "OBS STOP");
            AGVErrorModel err2 = new("2", "AGV-2", "EMC STOP", "OBS STOP");
            errorData.Add(err);
            errorData.Add(err2);


            gridViewDS.DataSource = showData;
            //gridViewError.DataSource = errorData;
        }

        public class AGVCallingModel
        {
            public string Time { get; set; }
            public string Name { get; set; }
            public string Station { get; set; }
            public string Status { get; set; }
            public AGVCallingModel(string time, string agvname, string deliv, string status)
            {
                this.Time = time;
                this.Name = agvname;
                this.Station = deliv;
                this.Status = status;
            }
        }

        public class AGVErrorModel
        {
            public string Time { get; set; }
            public string Name { get; set; }
            public string Error { get; set; }
            public string Obstacle { get; set; }
            public AGVErrorModel(string dateTimeNow, string agvName, string errorCode, string obscode)
            {
                this.Time = dateTimeNow;
                this.Name = agvName;
                this.Error = errorCode;
                this.Obstacle = obscode;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}