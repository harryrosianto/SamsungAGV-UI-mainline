using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;

namespace samsung_mainLine
{
    public partial class Form1 : Form
    {

        public List<AGVCallingModel> AGVData = new();
        public List<AGVErrorModel> AGVError = new();
        public int counts;
        public int xAgv, yAgv, interval = 5, cntStop = 0, cntStop4 = 0, counterLog, updateError = 0;
        DateTime dt = new DateTime();
        public string missionTime;
        public long jobId;
        public string agvState, agvName = "AGV-1", agvTime, agvStatus, agvRoute, agvRfid, statusDelivery, obsCode;
        private Color agvColor;
        public string agv2State, agv2Name = "AGV-2", agv2Time, agv2Status, agv2Route, agv2Rfid, statusDelivery2, obsCode2;
        public string AGVUniversalName;
        //public static string secondsValue = "";
        public int flags = 0;
        public double offTime, offTime2;
        public long btnState;
        public long btnState2;
        public long obsState;
        public long obsState2;
        public string readType;
        public string jobIDInput;
        public string errorCode, errorCode2, errorTime, errorTime2;
        public int traffic1;
        public int apiTraffic;
        public string trafficJob;
        public string jobData;
        public string SMDdata;

        public string url = "http://10.10.100.100:8000/req";

        private double batScale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
        public Form1()
        {   
            InitializeComponent();
            callAPI();
            modeButton.MouseHover += ModeButton_MouseHover;
            modeButton.MouseLeave += ModeButton_MouseLeave;
            logsButton.MouseHover += LogsButton_MouseHover;
            logsButton.MouseLeave += LogsButton_MouseLeave;
            timerButton.MouseHover += TimerButton_MouseHover;
            timerButton.MouseLeave += TimerButton_MouseLeave;
            homeButton.MouseHover += HomeButton_MouseHover;
            homeButton.MouseLeave += HomeButton_MouseLeave;
            giveUpButton.MouseHover += giveUpButton_MouseHover;
            giveUpButton.MouseLeave += giveUpButton_MouseLeave;
            trafficButton.MouseHover += trafficButton_MouseHover;
            trafficButton.MouseLeave += trafficButton_MouseLeave;

            timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
            timerData.Text = Convert.ToString(counts);

        }
        private async Task<ResponseData> API(string command)
        {
            var cmd = new RequestData();
            cmd.command = command;
            cmd.serialNumber = 0;
            var json = JsonConvert.SerializeObject(cmd);
            //Console.WriteLine(json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            ResponseData ret;
            //Console.Write("Response Data : {0}", data);
            while (true)
            {
                ret = JsonConvert.DeserializeObject<ResponseData>(response.Content.ReadAsStringAsync().Result);
                if (ret.command == cmd.command)
                {
                    break;
                }
            }
            return ret;
            //Console.WriteLine("Error Message: {0}", ret.errMark);
            //Console.WriteLine("Message: {0}", ret.msg);
            //Console.WriteLine("IP Robot: {0}", ret.msg[0][5][0]);
            //Console.WriteLine(ret);
            //string result = response.Content.ReadAsStringAsync().Result;
            ////Console.WriteLine(result);
        }

        private async Task<ResponseData2> APInonArray(string command)
        {
            var cmd = new RequestData();
            cmd.command = command;
            cmd.serialNumber = 0;

            var json = JsonConvert.SerializeObject(cmd);
            //Console.WriteLine(json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            ResponseData2 ret;
            //Console.Write("Response Data : {0}", data);
            while (true)
            {
                ret = JsonConvert.DeserializeObject<ResponseData2>(response.Content.ReadAsStringAsync().Result);
                if (ret.command == cmd.command)
                {
                    break;
                }
            }
            return ret;

        }

        private void activeLine(bool l1, bool l2, bool l3, bool l4, bool l5, bool l6)
        {
            this.Invoke(new MethodInvoker(delegate () {
                //send1.Visible = l1;
                //send2.Visible = l2;
                //send3.Visible = l3;
                //send4.Visible = l4;
                //send5.Visible = l5;
                //send6.Visible = l6;
                //standby1.Visible = !l1;
                //standby2.Visible = !l2;
                //standby3.Visible = !l3;
                //standby4.Visible = !l4;
                //standby5.Visible = !l5;
                //standby6.Visible = !l6;
                //wip1.Visible = l1;
                //wip2.Visible = l2;
                //wip3.Visible = l3;
                //wip4.Visible = l4;
                //wip5.Visible = l5;
                //wip6.Visible = l6;
            }));

        }

        class RequestData
        {
            public string command { get; set; }
            public int serialNumber { get; set; }
        }
        class ResponseData
        {
            public string errMark { get; set; }
            public List<List<dynamic>> msg { get; set; }        // add command checker
            public string command { get; set; }
        }
        class ResponseData2
        {
            public string errMark { get; set; }
            public List<dynamic> msg { get; set; }
            public string command { get; set; }
        }
        public class AGVDeviceModel
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public AGVDeviceModel(string agvId, string agvName, string status)
            {
                this.ID = agvId;
                this.Name = agvName;
                this.Status = status;
            }
        }
        public class AGV2DeviceModel
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public AGV2DeviceModel(string agvId, string agvName, string status)
            {
                this.ID = agvId;
                this.Name = agvName;
                this.Status = status;
            }
        }
        public class AGVStatusModel
        {
            //public string ID { get; set; }
            public string Name { get; set; }
            public string State { get; set; }
            public string Status { get; set; }
            public AGVStatusModel(string agvName, string power, string status)
            {
                //this.ID = agvId;
                this.Name = agvName;
                this.State = power;
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

        public class AGV2StatusModel
        {
            //public string ID { get; set; }
            public string Name { get; set; }
            public string State { get; set; }
            public string Status { get; set; }
            public AGV2StatusModel(string agvName, string power, string status)
            {
                //this.ID = agvId;
                this.Name = agvName;
                this.State = power;
                this.Status = status;
            }
        }
        public class AGV2CallingModel
        {
            public string Time { get; set; }
            public string Name { get; set; }
            public string Station { get; set; }
            public string Status { get; set; }
            public AGV2CallingModel(string time, string agvname, string deliv, string status)
            {
                this.Time = time;
                this.Name = agvname;
                this.Station = deliv;
                this.Status = status;
            }
        }
        public class AGV2ErrorModel
        {
            public string Time { get; set; }
            public string Name { get; set; }
            public string Error { get; set; }
            public string Obstacle { get; set; }
            public AGV2ErrorModel(string dateTimeNow, string agvName, string errorCode, string obscode)
            {
                this.Time = dateTimeNow;
                this.Name = agvName;
                this.Error = errorCode;
                this.Obstacle = obscode;
            }
        }

        private async void callAPI()
        {

            string[] arrayMachine1 = new string[] { "SMD_01", "SMD_02", "SMD_03", "SMD_04", "SMD_05", "SMD_06" };
            string[] arrayMachine2 = new string[] { "SMD_07", "SMD_08", "SMD_09", "SMD_10", "SMD_11", "SMD_12" };

            string[] arrayPosition = new string[] {"HOME2","HOME1", "TRAFFIC2", "TRAFFIC1","BRANCH2", "BRANCH1","WIPFULL2", "WIPFULL1", // --- 8rfid
                                                   "WIP-IN-6", "WIP-IN-5","WIP-IN-4","WIP-IN-3","WIP-IN-2","WIP-IN-1","BRANCHLINE", // --- 7rfid
                                                   "RELEASE1", "STRAIGHTLINE1","TRAFFICHOME1","STRAIGHTLINE1", "STRAIGHTLINE1", "STRAIGHTLINE1", "STRAIGHTLINE1","STRAIGHTLINE1","STRAIGHTLINE1","STRAIGHTLINE1", // --- 10rfid
                                                   "SMD01","SMD01","SMD02","SMD02","SMD03","SMD03","SMD04","SMD04","SMD05","SMD05","SMD06","SMD06", "ENDLINE", // --- 13rfid
                                                   "BRANCHHOME","RELEASE2","STRAIGHTLINE2","TRAFFICHOME2","STRAIGHTLINE2","STRAIGHTLINE2","STRAIGHTLINE2","STRAIGHTLINE2", // --- 8rfid
                                                   "SMD07","SMD07","SMD08","SMD08","SMD09","SMD09","SMD10","SMD10","SMD11","SMD11","SMD12","SMD12","ENDLINE"};

            string[] arrayRFID = new string[] { "128","129","126","127","46","131","13","130",                                                      //HOME to WIP --- 8rfid
                                                "125","124","123","122","121","120","2",                                                            //WIP to LINE --- 7rfid
                                                "119","1","117","11","56","54","53","52","31","32",                                                 //to Line 1-6 --- 10rfid
                                                "33", "34", "15","16","37","38","39","40","41","42","43","44","8",                                  //Line 1-6 --- 13rfid
                                                "30","118","101","116","48","49","102","103",                                                       //to Line 7-12 --- 8rfid
                                                "104","105","106","107","108,","109","110","111","112","113","114","115" ,"8"};                     //Line 7-12 --- 13rfid

            string[] arrayRFIDHorizontal = new string[] { "128", "129", "126", "127", "119", "1", "117", "11", "56", "54", "53", "52", "31", "32", "30", "118", "101", "116", "48", "49", "102" };
            string[] arrayRFIDVertical = new string[] { "46", "131", "13", "130", "33", "34", "15", "16", "37", "38", "39", "40", "41", "42", "43", "44", "8", "104", "105", "106", "107", "108,", "109", "110", "111", "112", "113", "114", "115", "8" };

            ResponseData data = await API("missionC.missionGetActiveList()");

            if (data.errMark == "ok")
            {
                //List<AGVCallingModel> showData = new();
                List<AGVCallingModel> showData = new List<AGVCallingModel>();
                List<AGV2CallingModel> showData2 = new List<AGV2CallingModel>();
                string lastTIme = "";
                for (int i = 0; i < data.msg.Count; i++)
                {
                    counterLog += 1;
                    agvTime = UnixTimeStampToDateTime(data.msg[i][11]).ToString();
                    statusDelivery = data.msg[i][10];
                    jobId = data.msg[i][0];
                    lastTIme = agvTime;
                    SMDdata = data.msg[i][1].ToString();
                    jobData = data.msg[i][0].ToString();



                    bool AGV1Deliver = arrayMachine1.Contains(SMDdata);
                    bool AGV2Deliver = arrayMachine2.Contains(SMDdata);
                    //string text = System.IO.File.ReadAllText(@"D:\WORK\SAMSUNG\SampleUI-SamsungAGV\log.txt");
                    //string[] lines = System.IO.File.ReadAllLines(@"D:\WORK\SAMSUNG\SampleUI-SamsungAGV\log.txt");


                    if (AGV1Deliver == true)
                    {
                        AGVUniversalName = agvName;
                    }
                    else if (AGV2Deliver == true)
                    {
                        AGVUniversalName = agv2Name;
                    }

                    if (counterLog < 11)
                    {
                        if (statusDelivery == "执行") { statusDelivery = "RUNNING"; }
                        else if (statusDelivery == "放弃") { statusDelivery = "HOLD"; }
                        else if (statusDelivery == "正常结束") { statusDelivery = "FINISH"; }
                        else if (statusDelivery == "错误")
                        {
                            string disc = AGVUniversalName + " DISCONNECTED";
                            if (labelDisconnect.InvokeRequired)
                            {
                                labelDisconnect.Invoke(new Action(callAPI));
                                return;
                            }
                            labelDisconnect.Text = disc;
                            labelDisconnect.Visible = true;
                        }
                        else
                        {
                            Console.WriteLine("Status Delivery : {0}", statusDelivery);
                            labelDisconnect.Visible = false;
                        }

                        //Console.WriteLine("{0} {1} {2} {3} {4} cnt : {5} {6} lastTime : {7}", agvTime, agvName, jobId, statusDelivery, data.msg.Count,
                        //counterLog, writeFlag, lastTIme);
                        //System.Console.WriteLine("Contents of WriteText.txt = \n{0}", text);
                        //foreach (string line in lines)
                        //{
                        //    // Use a tab to indent each line of the file.
                        //    //Console.WriteLine("\n" + line);
                        //}
                        //Console.WriteLine(lines[0]);

                        //// Display the file contents by using a foreach loop.
                        //System.Console.WriteLine("Contents of WriteLines2.txt = \n
                    }
                    else
                    { //Console.WriteLine("API 1 Else ");
                    }

                    if (statusDelivery == "执行")
                    {
                        statusDelivery = "RUNNING";
                        jobId = data.msg[i][0];
                        long[] arrayJobId = new long[] {data.msg[0][0], data.msg[1][0], data.msg[2][0], data.msg[3][0], data.msg[4][0],
                                            data.msg[5][0], data.msg[6][0], data.msg[7][0], data.msg[8][0],data.msg[9][0] };

                        long maxJobid = arrayJobId.Last(), searchJobid = jobId;
                        long indexJob = Array.IndexOf(arrayJobId, searchJobid);

                        //trafficJob = '2131'
                        trafficJob = data.msg[0][0].ToString();
                        Console.WriteLine("Job ID Traffic adalah {0}", trafficJob);

                        //string searchString = data.msg[(int)indexJob][1];               //Read first call jobID then search Name
                        //int index = Array.IndexOf(arrayMachine, searchString);
                        AGVCallingModel temp = new AGVCallingModel(agvTime, AGVUniversalName, SMDdata + " (" + jobData + ")", statusDelivery);
                        showData.Add(temp);
                        //if (index == 0) { activeLine(true, false, false, false, false, false);}
                        //else if (index == 1){ activeLine(false, true, false, false, false, false);}
                        //else if (index == 2){ activeLine(false, false, true, false, false, false);}
                        //else if (index == 3){ activeLine(false, false, false, true, false, false);}
                        //else if (index == 4){ activeLine(false, false, false, false, true, false);}
                        //else if (index == 5){ activeLine(false, false, false, false, false, true);}
                        //else { activeLine(false, false, false, false, false, false);}
                    }
                    else if (statusDelivery == "放弃")
                    {
                        statusDelivery = "HOLD";
                        AGVCallingModel temp = new AGVCallingModel(agvTime, AGVUniversalName, SMDdata + " (" + jobData + ")", statusDelivery);
                        //showData.Add(temp);
                    }
                    else if (statusDelivery == "正常结束")
                    {
                        statusDelivery = "FINISH";
                        AGVCallingModel temp = new AGVCallingModel(agvTime, AGVUniversalName, SMDdata + " (" + jobData + ")", statusDelivery);
                        showData.Add(temp);
                    }
                    else if (statusDelivery == "错误")
                    {
                        statusDelivery = "COM ERR";
                        AGVCallingModel temp = new AGVCallingModel(agvTime, AGVUniversalName, SMDdata + " (" + jobData + ")", statusDelivery);
                        showData.Add(temp);
                    }
                    else { activeLine(false, false, false, false, false, false); }
                }


                gridViewDS.Invoke((MethodInvoker)delegate {
                    gridViewDS.DataSource = showData;
                    //this.gridViewDS.Columns[0].Width = 120;
                    //this.gridViewDS.Columns[1].Width = 55;
                    //this.gridViewDS.Columns[2].Width = 100;
                });
            }

            data = await API("devC.getCarList()");

            if (data.errMark == "OK")
            {
                List<AGVStatusModel> showData = new List<AGVStatusModel>();


                for (int i = 0; i < data.msg.Count; i++)
                {
                    double power = data.msg[i][7];
                    double rfidNow = data.msg[i][33];

                    //"车" Read RFID and detail Car activity
                    double dataMovement = data.msg[i][15], dataRute = data.msg[i][31], dataRfid = data.msg[i][33], readAddress = data.msg[i][0];
                    readType = data.msg[i][2];
                    string searchRFID = dataRfid.ToString();

                    agvRoute = dataRute.ToString();

                    int agvRfid = (int)dataRfid;

                    if (readAddress == 1 && readType == "车" && i == 0)
                    {
                        if (dataMovement == 0) { agvStatus = "STOP"; agvColor = Color.Yellow; }
                        else if (dataMovement == 1) { agvStatus = "PAUSE"; agvColor = Color.Yellow; }
                        else if (dataMovement == 2) { agvStatus = "RUN"; agvColor = Color.Lime; }
                        else { }

                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;
                        AGV1StatusLabel.Text = agvStatus;

                        rfidLabel1.Text = rfidNow.ToString();

                        batteryLevel1.Value = (int)power;

                        if (batValue1.InvokeRequired)
                        {
                            batValue1.Invoke(new Action(callAPI));
                            return;
                        }
                        batValue1.Text = power.ToString();

                        int indexRFID1 = Array.IndexOf(arrayRFID, searchRFID);

                        bool horizontalTarget = arrayRFIDHorizontal.Contains(searchRFID);
                        bool verticalTarget = arrayRFIDVertical.Contains(searchRFID);
                    }

                    else if (readAddress == 2 && readType == "车" && i == 1)
                    {
                        if (dataMovement == 0) { agv2Status = "STOP"; agvColor = Color.Yellow; }
                        else if (dataMovement == 1) { agv2Status = "PAUSE"; agvColor = Color.Yellow; }
                        else if (dataMovement == 2) { agv2Status = "RUN"; agvColor = Color.Lime; }
                        else { }

                        agv2Vertical.BackColor = agvColor;
                        agv2Horizontal.BackColor = agvColor;
                        AGV2StatusLabel.Text = agv2Status;
                        batteryLevel2.Value = (int)power;

                        if (batValue2.InvokeRequired)
                        {
                            batValue2.Invoke(new Action(callAPI));
                            return;
                        }
                        batValue2.Text = power.ToString();
                    }
                }
                    
            }

            data = await API("devC.getDeviceList()");
            if (data.errMark == "OK")
            {
                double agvAddress, agvAddress2;
                List<AGVDeviceModel> showDevice = new List<AGVDeviceModel>();
                List<AGV2DeviceModel> showDevice2 = new List<AGV2DeviceModel>();

                for (int i = 0; i < data.msg.Count; i++)
                {
                    agvAddress = data.msg[0][3]; 
                    agvAddress2 = data.msg[0][3];
                    offTime = data.msg[0][6];
                    offTime2 = data.msg[0][6];
                    if ((agvAddress == 1) && readType == "车" && offTime >= 10)
                    {
                        agvState = "OFF";
                        string disc = "AGV-1" + " DISCONNECTED";
                        AGV1NameLabel.Text = agvName;
                        Console.WriteLine(disc);
                        AGV1StateLabel.Text = agvState;
                        labelDisconnect.Text = disc;
                        labelDisconnect.Visible = true;
                    }
                    else if (agvAddress == 1 && readType == "车" && offTime < 1)
                    {

                        agvState = "ON";
                        AGV1NameLabel.Text = agvName;
                        AGV1StateLabel.Text = "ON";
                        labelDisconnect.Visible = false;
                    }
                    else
                    {
                        agvState = "OFF";
                        AGV1NameLabel.Text = agvName;
                        string disc = "AGV-1" + " DISCONNECTED but strange";
                        Console.WriteLine(disc);
                        AGV1StateLabel.Text = agvState;
                        labelDisconnect.Text = disc;
                        labelDisconnect.Visible = true;
                    }
                    if ((agvAddress2 == 2) && readType == "车" && offTime2 >= 10)
                    {
                        agv2State = "OFF";
                        string disc = "AGV-2" + " DISCONNECTED";
                        AGV2NameLabel.Text = agv2Name;
                        Console.WriteLine(disc);
                        AGV2StateLabel.Text = agv2State;
                        labelDisconnect1.Text = disc;
                        labelDisconnect1.Visible = true;
                        detailPanel2.BackgroundColor = Color.FromArgb(255, 87, 38, 65);
                    }
                    else if (agvAddress2 == 2 && readType == "车" && offTime2 < 0.3)
                    {
                        agv2State = "ON";
                        AGV2NameLabel.Text = agv2Name;
                        AGV2StateLabel.Text = "ON";
                        labelDisconnect1.Visible = false;
                    }
                    else
                    {
                        agv2State = "OFF";
                        string disc = "AGV-2" + " DISCONNECTED";
                        AGV2NameLabel.Text = agv2Name;
                        Console.WriteLine(disc);
                        AGV2StateLabel.Text = agv2State;
                        labelDisconnect1.Text = disc;
                        labelDisconnect1.Visible = true;
                        detailPanel2.BackgroundColor = Color.FromArgb(255, 87, 38, 65);
                    }
                }

                List<AGVErrorModel> showError = new List<AGVErrorModel>();
                errorTime = DateTime.Now.ToString();
                errorTime2 = DateTime.Now.ToString();
                ResponseData2 datanonArray = await APInonArray("devC.deviceDic[1].optionsLoader.load(carLib.RAM.DEV.BTN_EMC)");
                ResponseData2 datanonArray2 = await APInonArray("devC.deviceDic[1].optionsLoader.load(carLib.RAM.DEV.BTN_EMC)");

                Console.WriteLine("KONDISI EMG SEKARANG: {0}", datanonArray.msg[1]);

                try
                {
                    btnState = datanonArray.msg[1];
                }
                catch
                {
                    btnState = 1;
                }
                try
                {
                    btnState2 = datanonArray2.msg[1];
                }
                catch
                {
                    btnState2 = 1;
                }

                if (datanonArray.errMark == "OK")
                {
                    errorTime = DateTime.Now.ToString();
                    if (btnState == 0)
                    {
                        errorCode = "EMC STOP";
                        agv1Horizontal.BackColor = Color.Red;
                        agv1Vertical.BackColor = Color.Red;
                        detailPanel1.BackgroundColor = Color.FromArgb(255, 87, 38, 65);
                        EMG1Label.Visible = true;
                    }
                    else if (btnState == 1)
                    {
                        errorCode = "-";
                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;
                        detailPanel1.BackgroundColor = Color.FromArgb(255, 50, 50, 71);
                        EMG1Label.Visible = false;
                    }
                }
                else
                {
                    errorTime = "";
                    errorCode = "";
                }

                if (datanonArray2.errMark == "OK")
                {
                    errorTime2 = DateTime.Now.ToString();
                    if (btnState2 == 0)
                    {
                        errorCode2 = "EMC STOP";
                        agv1Horizontal.BackColor = Color.Red;
                        agv1Vertical.BackColor = Color.Red;
                    }
                    else if (btnState2 == 1)
                    {
                        errorCode2 = "-";
                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;
                    }
                }
                else
                {
                    errorTime2 = "";
                    errorCode2 = "";
                }

                ResponseData2 nonArrayOBS = await APInonArray("devC.deviceDic[1].optionsLoader.load(carLib.RAM.DEV.OBS)");
                ResponseData2 nonArrayOBS2 = await APInonArray("devC.deviceDic[1].optionsLoader.load(carLib.RAM.DEV.OBS)");

                try
                {
                    obsState = nonArrayOBS.msg[2];
                }
                catch (NullReferenceException)
                {
                    obsState = 3;
                }

                try
                {
                    obsState2 = nonArrayOBS2.msg[2];
                }
                catch (NullReferenceException)
                {
                    obsState2 = 3;
                }

                if (nonArrayOBS.errMark == "OK")
                {

                    if (obsState == 7)
                    {
                        obsCode = "OBS STOP";
                        agv1Horizontal.BackColor = Color.Red;
                        agv1Vertical.BackColor = Color.Red;
                    }
                    else
                    {
                        obsCode = "-";
                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;
                    }
                }
                else
                {
                    errorTime = "";
                    obsCode = "";
                }

                if (nonArrayOBS2.errMark == "OK")
                {
                    if (obsState2 == 7)
                    {
                        obsCode2 = "OBS STOP";
                        agv1Horizontal.BackColor = Color.Red;
                        agv1Vertical.BackColor = Color.Red;
                    }
                    else
                    {
                        obsCode2 = "-";
                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;
                    }
                }
                else
                {
                    errorTime2 = "";
                    obsCode2 = "";
                }
                updateError = 0;

                Console.WriteLine("Kondisi OBS 1: {0}", obsState);
                Console.WriteLine("Kondisi OBS 2: {0}", obsState2);
                AGVErrorModel temp = new AGVErrorModel(errorTime, agvName, errorCode, obsCode);
                AGVErrorModel temp2 = new AGVErrorModel(errorTime2, agv2Name, errorCode2, obsCode2);
                showError.Add(temp);
                showError.Add(temp2);
                //gridViewError.Invoke((MethodInvoker)delegate { gridViewError.DataSource = showError; });

            }
            
            else if (data.errMark == "err")
            {
                agvState = "OFF";
                agv2State = "OFF";
                string disc = agvName + " DISCONNECTED";
                //Console.WriteLine(disc);
                labelDisconnect.Text = disc;
                labelDisconnect.Visible = true;
            }
            callAPI();

            //List<AGVErrorModel> errorData = new();
            //missionTime = DateTime.Now.ToString("HH:mm:ss");
            //timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");   
        }

        public class AGVCallingModel
        {
            public string Time { get; set; }
            public string Name { get; set; }
            public string Line { get; set; }
            public string Status { get; set; }
            public AGVCallingModel(string time, string agvname, string deliv, string status)
            {
                this.Time = time;
                this.Name = agvname;
                this.Line = deliv;
                this.Status = status;
            }
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void timerButton_Click(object sender, EventArgs e)
        {
            FormTimer f = new();
            f.ShowDialog();
            //flags = 1;
            counts = Convert.ToInt32(FormTimer.secondsValue);
            timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
            timerData.Text = FormTimer.secondsValue;
            //if (flags == 1)
            //{
            //    bunifuPanel3.BackColor = Color.Yellow;
            //}
        }
        public void homeButton_Click(object sender, EventArgs e)
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counts--;
            if (counts == 0)
            {
                timer1.Stop();
            }
            Console.WriteLine(flags);
            timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
        }

        private void modeButton_Click(object sender, EventArgs e)
        {
            lightMode lmForm = new lightMode();
            this.Hide();
            lmForm.Show();
        }

        

        private void ModeButton_MouseHover(object? sender, EventArgs e)
        {
            modeIndicator.Visible = true;
        }
        private void ModeButton_MouseLeave(object? sender, EventArgs e)
        {
            modeIndicator.Visible = false;
        }
        private void LogsButton_MouseHover(object? sender, EventArgs e)
        {
            logsIndicator.Visible = true;
        }
        private void LogsButton_MouseLeave(object? sender, EventArgs e)
        {
            logsIndicator.Visible = false;
        }
        private void TimerButton_MouseHover(object? sender, EventArgs e)
        {
            timerIndicator.Visible = true;
        }
        private void TimerButton_MouseLeave(object? sender, EventArgs e)
        {
            timerIndicator.Visible = false;
        }
        private void HomeButton_MouseHover(object? sender, EventArgs e)
        {
            homeIndicator.Visible = true;
        }
        private void HomeButton_MouseLeave(object? sender, EventArgs e)
        {
            homeIndicator.Visible = false;
        }
        private void giveUpButton_MouseHover(object? sender, EventArgs e)
        {
            giveIndicator.Visible = true;
        }
        private void giveUpButton_MouseLeave(object? sender, EventArgs e)
        {
            giveIndicator.Visible = false;
        }
        private void trafficButton_MouseHover(object? sender, EventArgs e)
        {
            trafficIndicator.Visible = true;
        }
        private void trafficButton_MouseLeave(object? sender, EventArgs e)
        {
            trafficIndicator.Visible = false;
        }
        private void bunifuLabel14_Click(object sender, EventArgs e)
        {
            
        }

        private void logsButton_Click(object sender, EventArgs e)
        {
            if (gridViewDS.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                string outputName = DateTime.Today.ToString("dd-mm-yyyy");
                sfd.FileName = "Logs Exported - " + outputName + ".csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("failed to export csv file" + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = gridViewDS.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[gridViewDS.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += gridViewDS.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < gridViewDS.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += gridViewDS.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("csv file exported successfully", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("no delivery status to export!", "Info");
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;

        }
        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            //87 38 65
            detailPanel1.BackgroundColor = Color.FromArgb(255, 87, 38, 65);
            EMG1Label.Visible = true;
        }

        private void bunifuFlatButton1_DoubleClick(object sender, EventArgs e)
        {
            // 50 50 71
            detailPanel1.BackgroundColor = Color.FromArgb(255, 50, 50, 71);
            EMG1Label.Visible = false;
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            //207 0 47
            frontSensor1.Visible = true;
            frontSensorIden1.BackgroundColor = Color.FromArgb(255, 207, 0, 47);
            frontSensor1Text.Text = "Detect Object";
        }

        private void bunifuFlatButton2_DoubleClick(object sender, EventArgs e)
        {
            //42 202 147
            frontSensor1.Visible = false;
            frontSensorIden1.BackgroundColor = Color.FromArgb(255, 42, 202, 147);
            frontSensor1Text.Text = "Clear";
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            rearSensor1.Visible = true;
            rearSensorIden1.BackgroundColor = Color.FromArgb(255, 207, 0, 47);
            rearSensor1Text.Text = "Detect Object";

        }

        private void bunifuFlatButton3_DoubleClick(object sender, EventArgs e)
        {
            rearSensor1.Visible = false;
            rearSensorIden1.BackgroundColor = Color.FromArgb(255, 42, 202, 147);
            rearSensor1Text.Text = "Clear";
        }

        private void gridViewDS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void giveUpButton_Click(object sender, EventArgs e)
        {
            givingUp g = new givingUp();
            g.ShowDialog();
            jobIDInput = givingUp.jobIDValue;
            string cancelJob = "missionC.netMissionCancel(" + jobIDInput + ")";
            await API(cancelJob);
        }
    }
}