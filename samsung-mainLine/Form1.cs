using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using System.Windows.Forms;

using System.Net;
using System.IO;
//using GroupDocs.Parser.Data;
//using GroupDocs.Parser.Options;
//using GroupDocs.Parser;
using System.Net.Http;
using System.Threading;
using System.Resources;



namespace samsung_mainLine
{
    public partial class Form1 : Form
    {
        System.Drawing.Point location = System.Drawing.Point.Empty;
        private System.Drawing.Point _mouseLoc;
        private static string m_exePath = string.Empty;
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
        public long obsStateFront1;
        public long obsStateRear1;
        public long obsState2;
        public long obsStateFront2;
        public long obsStateRear2;
        public string readType;
        public string jobIDInput;
        public string errorCode, errorCode2, errorTime, errorTime2;
        public int traffic1;
        public int apiTraffic;
        public string trafficJob;
        public string jobData;
        public string SMDdata;
        public int waitingTime = 0, moveCnt = 0;
        public double rfid2, route2;
        public string chargingTime;
        public string timeNow, endChargingTime;
        public string chargingTime2;
        public int cycleCount = 0, workHour1 = 0, workHour2 = 0;
        public int chargingStatus = 0;



        public string url = "http://10.10.100.100:8000/req";

        private double batScale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
        [Obsolete]
        public Form1()
        {   
            InitializeComponent();
            callAPI();
            modeButton.MouseHover += ModeButton_MouseHover;
            modeButton.MouseLeave += ModeButton_MouseLeave;
            logsButton1.MouseHover += logsButton1_MouseHover;
            logsButton1.MouseLeave += logsButton1_MouseLeave;
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
            startButton.MouseHover += startButton_MouseHover;
            startButton.MouseLeave += startButton_MouseLeave;
            startButton2.MouseHover += startButton2_MouseHover;
            startButton.MouseLeave += startButton2_MouseLeave;

            //timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
            //timerLabel2.Text = dt.AddSeconds(counts).ToString("mm:ss");
            timerData.Text = Convert.ToString(counts);
            timerData2.Text = Convert.ToString(counts);

            //timeNow = DateTime.Now.ToString("HH:mm:ss");
            //waktuLabel.Text = timeNow;
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

            string[] arrayMachine1 = new string[] { "ROW_1", "CHARGING_1" };
            string[] arrayMachine2 = new string[] { "ROW_2", "CHARGING_2" };

            string[] arrayPosition = new string[] {"HOME2","HOME1", "TRAFFIC2", "TRAFFIC1","BRANCH2", "BRANCH1","WIPFULL2", "WIPFULL1", // --- 8rfid
                                                   "WIP-IN-6", "WIP-IN-5","WIP-IN-4","WIP-IN-3","WIP-IN-2","WIP-IN-1","BRANCHLINE", // --- 7rfid
                                                   "RELEASE1", "STRAIGHTLINE1","TRAFFICHOME1","STRAIGHTLINE1", "STRAIGHTLINE1", "STRAIGHTLINE1", "STRAIGHTLINE1","STRAIGHTLINE1","STRAIGHTLINE1","STRAIGHTLINE1", // --- 10rfid
                                                   "SMD01","SMD01","SMD02","SMD02","SMD03","SMD03","SMD04","SMD04","SMD05","SMD05","SMD06","SMD06", "ENDLINE", // --- 13rfid
                                                   "BRANCHHOME","RELEASE2","STRAIGHTLINE2","TRAFFICHOME2","STRAIGHTLINE2","STRAIGHTLINE2","STRAIGHTLINE2","STRAIGHTLINE2", // --- 8rfid
                                                   "SMD07","SMD07","SMD08","SMD08","SMD09","SMD09","SMD10","SMD10","SMD11","SMD11","SMD12","SMD12","ENDLINE"};

            string[] arrayRFID = new string[] { "501","502","503","504","505",                                                              
                                                "601","602","603","604","605","606",
                                                "200","201","202","203","204","205","206","207","208","209","210","211","212","213",
                                                "200","2011","2021","2031","2041","2051","2061","207","2081","2091","2101","2111","2121","213",
                                                "100","101","102","103","104","105","106","107","108","109","110","111","112",
                                                "1031","1091",
                                                "301","302","303","304",
                                                "401","402","403","404"};             

            string[] arrayRFIDHorizontal = new string[] { "501","502","503","504","505",
                                                          "601","602","603","604","605","606",
                                                          "201","202","203","204","205","206","207","208","209","210","211","212","213",
                                                          "2011","2021","2031","2041","2051","2061","207","2081","2091","2101","2111","2121","213",
                                                          "100",
                                                          "1031","1091",
                                                          "301","302","303","304",
                                                          "401","402","403" };

            string[] arrayRFIDVertical = new string[] { "101", "102", "103", "104", "105", "106", "107", "108", "109", "110", "111", "112", "404", "200" };

            //timeNow = DateTime.Now.ToString("HH:mm:ss");
            //waktuLabel.Text = timeNow;

            ResponseData data = await API("missionC.missionGetActiveList()");

            if (data.errMark == "ok")
            {
                //List<AGVCallingModel> showData = new();
                List<AGVCallingModel> showData = new List<AGVCallingModel>();
                List<AGVCallingModel> showData2 = new List<AGVCallingModel>();
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
                        //    Use a tab to indent each line of the file.
                        //    Console.WriteLine("\n" + line);
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
                        if (AGVUniversalName == agvName)
                        {
                            showData.Add(temp);
                        }
                        else if (AGVUniversalName == agv2Name)
                        {
                            showData2.Add(temp);
                        }
                        //showData.Add(temp);
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
                        //showData.Add(temp);

                        if (AGVUniversalName == agvName)
                        {
                            showData.Add(temp);
                        }
                        else if (AGVUniversalName == agv2Name)
                        {
                            showData2.Add(temp);
                            
                        }
                    }
                    else if (statusDelivery == "错误")
                    {
                        statusDelivery = "COM ERR";
                        AGVCallingModel temp = new AGVCallingModel(agvTime, AGVUniversalName, SMDdata + " (" + jobData + ")", statusDelivery);
                        //showData.Add(temp);

                        if (AGVUniversalName == agvName)
                        {
                            showData.Add(temp);
                        }
                        else if (AGVUniversalName == agv2Name)
                        {
                            showData2.Add(temp);
                        }
                    }
                    else { activeLine(false, false, false, false, false, false); }
                }


                gridViewDS.Invoke((MethodInvoker)delegate {
                    gridViewDS.DataSource = showData;
                    //this.gridViewDS.Columns[0].Width = 120;
                    //this.gridViewDS.Columns[1].Width = 55;
                    //this.gridViewDS.Columns[2].Width = 100;
                });

                gridViewDS2.Invoke((MethodInvoker)delegate {
                    gridViewDS2.DataSource = showData2;
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
                    double routeNow = data.msg[i][31];
                    Console.WriteLine(power);

                    //"车" Read RFID and detail Car activity
                    double dataMovement = data.msg[i][15], readAddress = data.msg[i][0];
                    readType = data.msg[i][2];
                    string searchRFID = rfidNow.ToString();

                    agvRoute = routeNow.ToString();

                    int agvRfid = (int)rfidNow;

                    //chargingTime = "11:32:00";
                    //endChargingTime = "12:35:00";

                    if (readAddress == 1 && readType == "车")
                    {
                        if (dataMovement == 0) { agvStatus = "STOP"; agvColor = Color.Yellow; }
                        else if (dataMovement == 1) { agvStatus = "PAUSE"; agvColor = Color.Yellow; }
                        else if (dataMovement == 2) { agvStatus = "RUN"; agvColor = Color.Lime; }
                        else { }

                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;
                        AGV1StatusLabel.Text = agvStatus;

                        rfidLabel1.Text = rfidNow.ToString();
                        routeLabel1.Text = routeNow.ToString();

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

                        if (rfidNow == 501)
                        {
                            //timer1 = new System.Windows.Forms.Timer();
                            //timer1.Tick += new EventHandler(timer1_Tick);
                            //timer1.Interval = 1000; // 1 second
                            //timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
                            //timer1.Start();
                        }

                    }

                    if (readAddress == 2 && readType == "车")
                    {
                        if (dataMovement == 0) { agv2Status = "STOP"; agvColor = Color.Yellow; }
                        else if (dataMovement == 1) { agv2Status = "PAUSE"; agvColor = Color.Yellow; }
                        else if (dataMovement == 2) { agv2Status = "RUN"; agvColor = Color.Lime; }
                        else { }

                        agv2Vertical.BackColor = agvColor;
                        agv2Horizontal.BackColor = agvColor;
                        AGV2StatusLabel.Text = agv2Status;

                        rfidLabel2.Text = rfid2.ToString();
                        routeLabel2.Text = route2.ToString();
                        

                        if (data.msg.Count == 2)
                        {
                            double power2 = data.msg[1][7];
                            double rfidNow2 = data.msg[1][33];
                            double routeNow2 = data.msg[1][31];

                            rfid2 = rfidNow2;
                            route2 = routeNow2;

                            batteryLevel2.Value = (int)power2;

                            if (batValue2.InvokeRequired)
                            {
                                batValue2.Invoke(new Action(callAPI));
                                return;
                            }
                            batValue2.Text = power2.ToString();
                        }
                        else
                        {
                            rfid2 = rfidNow;
                            route2 = routeNow;

                            batteryLevel2.Value = (int)power;

                            if (batValue2.InvokeRequired)
                            {
                                batValue2.Invoke(new Action(callAPI));
                                return;
                            }
                            batValue2.Text = power.ToString();
                        }

                        if (rfid2 == 213 && route2 == 210)
                        {

                            cycleCount = cycleCount + 1;
                            cycleLabel2.Text = cycleCount.ToString();
                            palette7.Visible = false;

                            try
                            {
                                await API("missionC.netMissionAdd('ROW_2')");
                            }
                            catch (Newtonsoft.Json.JsonSerializationException)
                            {
                                Console.WriteLine("Start Mission");
                            }
                            //timer1 = new System.Windows.Forms.Timer();
                            //timer1.Tick += new EventHandler(timer1_Tick);
                            //timer1.Interval = 1000; // 1 second
                            //timerLabel2.Text = dt.AddSeconds(counts).ToString("mm:ss");
                            //timer1.Start();
                        }

                        else if (rfid2 == 601)
                        {
                            agv2Horizontal.Left = rfid601.Left;
                            agv2Horizontal.Top = rfid601.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                            chargeIden2.Visible = true;
                        }
                        else if (rfid2 == 602)
                        {
                            agv2Horizontal.Left = rfid602.Left;
                            agv2Horizontal.Top = rfid602.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                            chargeIden2.Visible = false;
                        }
                        else if (rfid2 == 213)
                        {
                            agv2Horizontal.Left = rfid213.Left;
                            agv2Horizontal.Top = rfid213.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                            chargeIden2.Visible = false;
                        }
                        else if (rfid2 == 207)
                        {
                            agv2Horizontal.Left = rfid207.Left;
                            agv2Horizontal.Top = rfid207.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                            palette8.Visible = false;
                            if (route2 == 210)
                            {
                                palette7.Visible = true;
                            }
                        }
                        else if (rfid2 == 208)
                        {
                            agv2Horizontal.Left = rfid208.Left;
                            agv2Horizontal.Top = rfid208.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                            palette9.Visible = false;
                            if (route2 == 210)
                            {
                                palette8.Visible = true;
                            }
                        }
                        else if (rfid2 == 209)
                        {
                            agv2Horizontal.Left = rfid209.Left;
                            agv2Horizontal.Top = rfid209.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                            palette10.Visible = false;
                            if (route2 == 210)
                            {
                                palette9.Visible = true;
                            }
                        }
                        else if (rfid2 == 210)
                        {
                            agv2Horizontal.Left = rfid210.Left;
                            agv2Horizontal.Top = rfid210.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                            palette11.Visible = false;
                            if (route2 == 210)
                            {
                                palette10.Visible = true;
                            }
                        }
                        else if (rfid2 == 211)
                        {
                            agv2Horizontal.Left = rfid211.Left;
                            agv2Horizontal.Top = rfid211.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                            palette12.Visible = false;
                            if (route2 == 209)
                            {
                                palette11.Visible = true;
                            }
                            else
                            {
                                palette11.Visible = false;
                            }
                        }
                        else if (rfid2 == 212)
                        {
                            agv2Horizontal.Left = rfid212.Left;
                            agv2Horizontal.Top = rfid212.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                            if (route2 == 209)
                            {
                                palette12.Visible |= true;
                            }
                        }
                        else if (rfid2 == 604)
                        {
                            agv2Horizontal.Left = rfid604.Left;
                            agv2Horizontal.Top = rfid604.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                        }
                        else if (rfid2 == 606)
                        {
                            agv2Horizontal.Left = rfid606.Left;
                            agv2Horizontal.Top = rfid606.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                        }
                        else if (rfid2 == 402)
                        {
                            agv2Horizontal.Left = rfid402.Left;
                            agv2Horizontal.Top = rfid402.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                        }
                        else if (rfid2 == 403)
                        {
                            agv2Horizontal.Left = rfid403.Left;
                            agv2Horizontal.Top = rfid403.Top;
                            agv2Horizontal.Visible = true;
                            agv2Vertical.Visible = false;
                        }
                        else if (rfid2 == 200)
                        {
                            agv2Vertical.Left = rfid200.Left;
                            agv2Vertical.Top = rfid200.Top;
                            agv2Horizontal.Visible = false;
                            agv2Vertical.Visible = true;
                        }
                        else if (rfid2 == 404)
                        {
                            agv2Vertical.Left = rfid404.Left;
                            agv2Vertical.Top = rfid404.Top;
                            agv2Horizontal.Visible = false;
                            agv2Vertical.Visible = true;
                        }
                        else if (rfid2 == 107)
                        {
                            agv2Vertical.Left = rfid107.Left;
                            agv2Vertical.Top = rfid107.Top;
                            agv2Horizontal.Visible = false;
                            agv2Vertical.Visible = true;
                            if (route2 == 205)
                            {
                                station7.Visible = true;
                            }
                        }
                        else if (rfid2 == 108)
                        {
                            agv2Vertical.Left = rfid108.Left;
                            agv2Vertical.Top = rfid108.Top;
                            agv2Horizontal.Visible = false;
                            agv2Vertical.Visible = true;
                            station7.Visible = false;
                            if (route2 == 205)
                            {
                                station8.Visible = true;
                            }
                        }
                        else if (rfid2 == 109)
                        {
                            agv2Vertical.Left = rfid109.Left;
                            agv2Vertical.Top = rfid109.Top;
                            agv2Horizontal.Visible = false;
                            agv2Vertical.Visible = true;
                            station8.Visible = false;
                            if (route2 == 205)
                            {
                                station9.Visible = true;
                            }
                        }
                        else if (rfid2 == 110)
                        {
                            agv2Vertical.Left = rfid110.Left;
                            agv2Vertical.Top = rfid110.Top;
                            agv2Horizontal.Visible = false;
                            agv2Vertical.Visible = true;
                            station9.Visible = false;
                            if (route2 == 205)
                            {
                                station10.Visible = true;
                            }
                        }
                        else if (rfid2 == 111)
                        {
                            agv2Vertical.Left = rfid111.Left;
                            agv2Vertical.Top = rfid111.Top;
                            agv2Horizontal.Visible = false;
                            agv2Vertical.Visible = true;
                            station10.Visible = false;
                            station12.Visible = false;
                            if (route2 == 205)
                            {
                                station11.Visible = true;
                            }
                        }
                        else if (rfid2 == 112)
                        {
                            agv2Vertical.Left = rfid112.Left;
                            agv2Vertical.Top = rfid112.Top;
                            agv2Horizontal.Visible = false;
                            agv2Vertical.Visible = true;
                            station11.Visible = false;
                            if (route2 == 205)
                            {
                                station12.Visible = true;
                            }
                        }
                        else
                        {
                            station7.Visible = false;
                            station8.Visible = false;
                            station9.Visible = false;
                            station10.Visible = false;
                            station11.Visible = false;
                            station12.Visible = false;
                        }
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
                    if (data.msg.Count == 2)
                    {
                        agvAddress = data.msg[0][3];
                        agvAddress2 = data.msg[1][3];

                        offTime = data.msg[0][6];
                        offTime2 = data.msg[1][6];
                    }
                    else
                    {
                        agvAddress = data.msg[i][3];
                        agvAddress2 = data.msg[i][3];

                        offTime = data.msg[i][6];
                        offTime2 = data.msg[i][6];
                    }

                    Console.WriteLine(agvAddress);
                    Console.WriteLine(readType);

                    if (agvAddress == 1 && readType == "车" && offTime >= 10)
                    {
                        agvState = "OFF";
                        string disc = "AGV-1" + " DISCONNECTED";
                        AGV1NameLabel.Text = agvName;
                        Console.WriteLine(disc);
                        AGV1StateLabel.Text = agvState;
                        labelDisconnect.Text = disc;
                        labelDisconnect.Visible = true;
                        detailPanel1.BackgroundColor = Color.FromArgb(255, 87, 38, 65);
                        timer1.Enabled = false;
                    }
                    else if (agvAddress == 1 && readType == "车" && offTime < 1)
                    {

                        agvState = "ON";
                        AGV1NameLabel.Text = agvName;
                        AGV1StateLabel.Text = "ON";
                        labelDisconnect.Visible = false;
                        detailPanel1.BackgroundColor = Color.FromArgb(255, 50, 50, 71);
                        timer1.Enabled = true;
                    }
                    else
                    {
                        agvState = "OFF";
                        AGV1NameLabel.Text = agvName;
                        string disc = "AGV-1" + " DISCONNECTED";
                        Console.WriteLine(disc);
                        AGV1StateLabel.Text = agvState;
                        labelDisconnect.Text = disc;
                        labelDisconnect.Visible = true;
                        detailPanel1.BackgroundColor = Color.FromArgb(255, 87, 38, 65);
                        timer1.Enabled = false;
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
                        timer2.Enabled = false;
                    }
                    else if (agvAddress2 == 2 && readType == "车" && offTime2 < 1)
                    {
                        agv2State = "ON";
                        AGV2NameLabel.Text = agv2Name;
                        AGV2StateLabel.Text = "ON";
                        labelDisconnect1.Visible = false;
                        detailPanel2.BackgroundColor = Color.FromArgb(255, 50, 50, 71);
                        timer2.Enabled = true;
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
                        timer2.Enabled = false;
                    }
                }

                errorTime = DateTime.Now.ToString();
                errorTime2 = DateTime.Now.ToString();
                ResponseData2 datanonArray = await APInonArray("devC.deviceDic[1].optionsLoader.load(carLib.RAM.DEV.BTN_EMC)");
                ResponseData2 datanonArray2 = await APInonArray("devC.deviceDic[2].optionsLoader.load(carLib.RAM.DEV.BTN_EMC)");

                Console.WriteLine("KONDISI EMG SEKARANG: {0}", btnState2);

                List<AGVErrorModel> showError = new List<AGVErrorModel>();

                try
                {
                    btnState = datanonArray.msg[1];
                }
                catch (NullReferenceException)
                {
                    btnState = 1;
                }

                try
                {
                    btnState2 = datanonArray2.msg[1];
                }
                catch (NullReferenceException)
                {
                    btnState2 = 1;
                }

                if (datanonArray.errMark == "OK")
                {
                    //List<AGVErrorModel> showError = new List<AGVErrorModel>();
                    errorTime = DateTime.Now.ToString();
                    if (btnState == 0)
                    {
                        errorCode = "EMC STOP";
                        agv1Horizontal.BackColor = Color.Red;
                        agv1Vertical.BackColor = Color.Red;
                        detailPanel1.BackgroundColor = Color.FromArgb(255, 87, 38, 65);
                        EMG1Label.Visible = true;

                        AGVErrorModel temp1 = new AGVErrorModel(errorTime, agvName, errorCode, obsCode);
                        showError.Add(temp1);
                    }
                    else if (btnState == 1)
                    {
                        errorCode = "-";
                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;
                        detailPanel1.BackgroundColor = Color.FromArgb(255, 50, 50, 71);
                        EMG1Label.Visible = false;
                    }

                    //gridViewError.Invoke((MethodInvoker)delegate { gridViewError.DataSource = showError; });
                }
                else
                {
                    errorTime = "";
                    errorCode = "";
                }

                if (datanonArray2.errMark == "OK")
                {
                    //List<AGVErrorModel> showError2 = new List<AGVErrorModel>();

                    errorTime2 = DateTime.Now.ToString();
                    if (btnState2 == 0)
                    {
                        errorCode2 = "EMC STOP";
                        agv1Horizontal.BackColor = Color.Red;
                        agv1Vertical.BackColor = Color.Red;
                        detailPanel2.BackgroundColor = Color.FromArgb(255, 87, 38, 65);
                        EMG2Label.Visible = true;

                        AGVErrorModel temp2 = new AGVErrorModel(errorTime, agvName, errorCode, obsCode);
                        showError.Add(temp2);
                    }
                    else if (btnState2 == 1)
                    {
                        errorCode2 = "-";
                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;
                        detailPanel2.BackgroundColor = Color.FromArgb(255, 50, 50, 71);
                        EMG2Label.Visible = false;
                    }

                    //gridViewError.Invoke((MethodInvoker)delegate { gridViewError.DataSource = showError2; });
                }

                ResponseData2 nonArrayOBS = await APInonArray("devC.deviceDic[1].optionsLoader.load(carLib.RAM.DEV.OBS)");
                ResponseData2 nonArrayOBS2 = await APInonArray("devC.deviceDic[2].optionsLoader.load(carLib.RAM.DEV.OBS)");

                try
                {
                    obsState = nonArrayOBS.msg[2];
                    obsStateFront1 = nonArrayOBS.msg[5];
                    obsStateRear1 = nonArrayOBS.msg[6];
                }
                catch (NullReferenceException)
                {
                    obsState = 3;
                    obsStateFront1 = 3;
                    obsStateRear1 = 3;
                }

                try
                {
                    obsState2 = nonArrayOBS2.msg[2];
                    obsStateFront2 = nonArrayOBS2.msg[5];
                    obsStateRear2 = nonArrayOBS2.msg[6];
                }
                catch (NullReferenceException)
                {
                    obsState2 = 3;
                    obsStateFront2 = 3;
                    obsStateRear2 = 3;
                }

                if (nonArrayOBS.errMark == "OK")
                {
                    //List<AGVErrorModel> showError = new List<AGVErrorModel>();

                    if (obsState == 7)
                    {
                        //obsCode = "OBS STOP";
                        agv1Horizontal.BackColor = Color.Red;
                        agv1Vertical.BackColor = Color.Red;

                        if (obsStateFront1 == 7)
                        {
                            obsCode = "OBS FRONT";
                            frontSensor1.Visible = true;
                            frontSensorIden1.BackgroundColor = Color.Red;
                            frontSensorText1.Text = "Detect Object";

                            AGVErrorModel temp3 = new AGVErrorModel(errorTime2, agv2Name, errorCode2, obsCode2);
                            showError.Add(temp3);
                        }

                        else if (obsStateRear1 == 7)
                        {
                            obsCode = "OBS REAR";
                            rearSensor1.Visible = true;
                            rearSensorIden1.BackgroundColor = Color.Red;
                            rearSensorText1.Text = "Detect Object";

                            AGVErrorModel temp4 = new AGVErrorModel(errorTime2, agv2Name, errorCode2, obsCode2);
                            showError.Add(temp4);
                        }

                        //gridViewError.Invoke((MethodInvoker)delegate { gridViewError.DataSource = showError; });
                    }
                    else
                    {
                        obsCode = "-";
                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;
                        frontSensor1.Visible = false;
                        frontSensorIden1.BackgroundColor = Color.FromArgb(255, 42, 202, 147);
                        frontSensorText1.Text = "Clear";

                        rearSensor1.Visible = false;
                        rearSensorIden1.BackgroundColor = Color.FromArgb(255, 42, 202, 147);
                        rearSensorText1.Text = "Clear";
                    }

                    //gridViewError.Invoke((MethodInvoker)delegate { gridViewError.DataSource = showError; });
                }

                if (nonArrayOBS2.errMark == "OK")
                {
                    //List<AGVErrorModel> showError = new List<AGVErrorModel>();

                    if (obsState2 == 7)
                    {
                        //obsCode2 = "OBS STOP";
                        agv1Horizontal.BackColor = Color.Red;
                        agv1Vertical.BackColor = Color.Red;

                        if (obsStateFront2 == 7)
                        {
                            obsCode2 = "OBS FRONT";
                            frontSensor2.Visible = true;
                            frontSensorIden2.BackgroundColor = Color.Red;
                            frontSensorText2.Text = "Detect Object";

                            AGVErrorModel temp5 = new AGVErrorModel(errorTime2, agv2Name, errorCode2, obsCode2);
                            showError.Add(temp5);
                        }

                        else if (obsStateRear2 == 7)
                        {
                            obsCode2 = "OBS REAR";
                            rearSensor2.Visible = true;
                            rearSensorIden2.BackgroundColor = Color.Red;
                            rearSensorText2.Text = "Detect Object";

                            AGVErrorModel temp6 = new AGVErrorModel(errorTime2, agv2Name, errorCode2, obsCode2);
                            showError.Add(temp6);
                        }
                        
                    }
                    else
                    {
                        obsCode2 = "-";
                        agv1Vertical.BackColor = agvColor;
                        agv1Horizontal.BackColor = agvColor;

                        frontSensor1.Visible = false;
                        frontSensorIden1.BackgroundColor = Color.FromArgb(255, 42, 202, 147);
                        frontSensorText1.Text = "Clear";

                        rearSensor1.Visible = false;
                        rearSensorIden1.BackgroundColor = Color.FromArgb(255, 42, 202, 147);
                        rearSensorText1.Text = "Clear";

                        frontSensor2.Visible = false;
                        frontSensorIden2.BackgroundColor = Color.FromArgb(255, 42, 202, 147);
                        frontSensorText2.Text = "Clear";
                        
                        rearSensor2.Visible = false;
                        rearSensorIden2.BackgroundColor = Color.FromArgb(255, 42, 202, 147);
                        rearSensorText2.Text = "Clear";
                    }

                    //gridViewError.Invoke((MethodInvoker)delegate { gridViewError.DataSource = showError; });
                }
                //updateError = 0;

                Console.WriteLine("Kondisi OBS 1: {0}", obsState);
                Console.WriteLine("Kondisi OBS 2: {0}", obsState2);
                //AGVErrorModel temp = new AGVErrorModel(errorTime, agvName, errorCode, obsCode);
                //AGVErrorModel temp2 = new AGVErrorModel(errorTime2, agv2Name, errorCode2, obsCode2);
                //showError.Add(temp);
                //showError.Add(temp2);
                gridViewError.Invoke((MethodInvoker)delegate { gridViewError.DataSource = showError; });

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

            //gridViewError.Invoke((MethodInvoker)delegate { gridViewError.DataSource = showError; });

            callAPI();

            //List<AGVErrorModel> errorData = new();
            //missionTime = DateTime.Now.ToString("HH:mm:ss");
            //timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");   
        }

        private async void timer5_Tick(object sender, EventArgs e)
        {
            waitingTime += 1;
            timeNow = DateTime.Now.ToString("HH:mm:ss");
            waktuLabel.Text = timeNow;

            chargingTime = timerLabel.Text;
            chargingTime2 = timerLabel2.Text;

            if (waktuLabel.Text == chargingTime || waktuLabel.Text == chargingTime2)
            {
                try
                {
                    await API("missionC.netMissionAdd('CHARGING_2')");
                }
                catch (Newtonsoft.Json.JsonSerializationException)
                {
                    Console.WriteLine("Start Mission");
                }
            }
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
            timerLabel.Text = FormTimer.secondsValue;
            timerLabel2.Text = FormTimer.secondsValue2;
            timerData.Text = FormTimer.secondsValue;
            timerData2.Text = FormTimer.secondsValue2;
            //if (flags == 1)
            //{
            //    bunifuPanel3.BackColor = Color.Yellow;
            //}
        }

        private void homeButton_DoubleClick(object sender, EventArgs e)
        {
            //timer2.Enabled = true;
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                await API("missionC.netMissionAdd('ROW_1')");
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                Console.WriteLine("Start Mission");
            }
        }

        public void homeButton_Click(object sender, EventArgs e)
        {
            timer2.Stop();
        }

        private async void startButton2_Click(object sender, EventArgs e)
        {
            try
            {
                await API("missionC.netMissionAdd('ROW_2')");
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                Console.WriteLine("Start Mission");
            }
        }

        private async void trafficButton_Click(object sender, EventArgs e)
        {
            try
            {
                await API("missionC.netMissionAdd('CLEAR_TRAFFIC')");
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                Console.WriteLine("Clear Traffic");
            }
        }
    
        private async void timer1_Tick(object sender, EventArgs e)
        {
            workHour1++;
            whoursLabel1.Text = dt.AddSeconds(workHour1).ToString("HH:mm:ss");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            workHour2++;
            whoursLabel2.Text = dt.AddSeconds(workHour2).ToString("HH:mm:ss");
            
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
        private void logsButton1_MouseHover(object? sender, EventArgs e)
        {
            logsIndicator1.Visible = true;
        }
        private void logsButton1_MouseLeave(object? sender, EventArgs e)
        {
            logsIndicator1.Visible = false;
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
        private void startButton_MouseHover(object? sender, EventArgs e)
        {
            startIndicator.Visible = true;
        }
        private void startButton_MouseLeave(object? sender, EventArgs e)
        {
            startIndicator.Visible = false;
        }
        private void startButton2_MouseHover(object? sender, EventArgs e)
        {
            startIndicator2.Visible = true;
        }
        private void startButton2_MouseLeave(object? sender, EventArgs e)
        {
            startIndicator2.Visible = false;
        }

        private void bunifuLabel14_Click(object sender, EventArgs e)
        {
            
        }

        private void logsButton_Click(object sender, EventArgs e)
        {
            if (gridViewDS2.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                string outputName = DateTime.Today.ToString("ddMMyyyy");
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
                            int columnCount = gridViewDS2.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[gridViewDS2.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += gridViewDS2.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < gridViewDS2.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += gridViewDS2.Rows[i - 1].Cells[j].Value.ToString() + ",";
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