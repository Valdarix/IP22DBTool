using System.Data;
using Microsoft.VisualBasic.FileIO;
using BingMapsRESTToolkit;
using System.Text;
using System.Globalization;
using Microsoft.Web.WebView2.Core;

namespace IndyPro22DatabaseManagementTool
{
    public partial class Form1 : Form
    {
        string filePath = string.Empty;
        enum SocialMedia
        {
            FaceBook,
            Twitter, 
            Instgram,
            Twitch,
            TikTok,
            URL,
            YouTube
        }

        SocialMedia currentSM;

        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            
        }
        private void Form_Resize(object sender, EventArgs e)
        {
            webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            goButton.Left = this.ClientSize.Width - goButton.Width;
            addressBar.Width = goButton.Left - addressBar.Left;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private DataTable rawData;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (filePath == string.Empty)
            {
                GetFilePath();
                return;
            }

            switch (listBox1.SelectedIndex)
            {
                case 0:
                    listView1.Clear();
                    var reader = DB.GetDataReader($"Select * from Talent order by Name COLLATE NOCASE Asc ");
                    //var reader = DB.GetDataReader($"Select * from Talent where Talent_ID NOT IN (Select ID FROM TalentBio) order by Name COLLATE NOCASE Asc ");                  
                    foreach (DataRow row in reader.Rows)
                    {                    
                      
                        listView1.Items.Add(DB.GetStrData(row, "Name"));
                        if (listView1.Items.Count == 0) return;
                        listView1.Items[0].Selected = true;
                        listView1.Select();
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;                    
                 
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            addressBar.Clear();
            rawDataList.Items.Clear();
            edtBio.Clear();
            edtLat.Clear();
            edtLon.Clear();
            if (listView1.SelectedItems.Count == 0)
                return;
            var selected = listView1.SelectedItems[0].Text;
            selected = selected.Replace("'", "''");
            //Now load the talent. 
            var reader = DB.GetDataReader($"Select * from Talent left join TalentBio on TALENT_ID = ID where Name = '{selected}'");
            foreach (DataRow row in reader.Rows)
            {
                var t = DB.LoadTalent(row);
                edtID.Text = t.ID.ToString();
                edtName.Text = t.name;
                edtNick.Text = t.nickNames;
                edtCity.Text = t.city;
                cbState.SelectedItem = t.state;
                edtTheme.Text = t.theme;
                edtFB.Text = t.Facebook;
                edtTW.Text = t.twitterHandle;
                edtIG.Text = t.Instagram;
                edtTT.Text = t.TikTok;
                edtYT.Text = t.Youtube;
                edtTWC.Text = t.Twitch;
                edtURL.Text = t.website;
                cbHeel.Checked = t.Heel > 0;
                cbFace.Checked = t.Face > 0;
                cbInRing.Checked = t.InRing > 0;
                cbBackstage.Checked = t.Backstage > 0;
                cbBooker.Checked = t.Booker > 0;
                cbAnnouncer.Checked = t.Announce > 0;
                cbColor.Checked = t.Color > 0;
                cbMGR.Checked = t.Mgr > 0;
                cbBrawler.Checked = t.Brawler > 0;
                cbGrappler.Checked = t.Grappler > 0;
                cbHighFlying.Checked = t.Flyer > 0;
                cbDaredevil.Checked = t.Daredevil > 0;
                cbLucha.Checked = t.Lucha > 0;
                cbStrongStyle.Checked = t.StrongStyle > 0;
                cbMMA.Checked = t.MMA > 0;
                cbHardcore.Checked = t.Hardcore > 0;
                cbDeathMatch.Checked = t.Deathmatch > 0;
                cbPrank.Checked = t.Prankster > 0;
                cbLeader.Checked = t.Leader > 0;
                cbInandOut.Checked = t.InAndOut > 0;
                cbSE.Checked = t.StraightEdge > 0;
                cbDrinks.Checked = t.Drinks > 0;
                cbSmokes.Checked = t.Smokes > 0;
                cbBully.Checked = t.Bully > 0;
                cbHelps.Checked = t.Helps > 0;
                cbWatches.Checked = t.Watches > 0;
                cbCreative.Checked = t.Creative > 0;
                edtBMth.Text = t.birthMonth;
                edtByr.Text = t.birthYear;
                edtdMth.Text = t.debutMonth;
                edtDyr.Text = t.debutYear;
                edtBio.Text = t.Bio;
                cbRef.Checked = t.Ref > 0;
                cbOldSchool.Checked = t.OldSchool > 0;
                cbCruiserweight.Checked = t.Cruiser > 0;

            }
            var reader1 = DB.GetDataReader($"Select * from RawTalent where RingName like '%{selected}%'");
            if (reader1.Rows.Count > 0)
            {
                lblRaw.BackColor = Color.Green;
                lblRaw.Text = $"Data found in the Raw Table {reader1.Rows.Count}";
                rawData = reader1;
                LoadRawData();
            }
            else
            {
                lblRaw.BackColor = Color.White;
                lblRaw.Text = "No Raw Data Found";
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            GetLatLong();
        }

        public async void GetLatLong()
        {
            var req = new GeocodeRequest();
            req.BingMapsKey = "AvrksM67bdtn2IAeV-Ovkm2DwWbYaNPa6xucbqQBVK8o081MQCPUF4MYvEDfpnD0";
            req.Query = $"{edtCity.Text.Trim()}, {cbState.Text.Trim()}";
            var result = await req.Execute();
            if (result.StatusCode == 200)
            {
                var toolkitLocation = (result?.ResourceSets?.FirstOrDefault())
               ?.Resources?.FirstOrDefault()
               as BingMapsRESTToolkit.Location;
                var latitude = toolkitLocation.Point.Coordinates[0];
                var longitude = toolkitLocation.Point.Coordinates[1];
                edtLat.Text = latitude.ToString();
                edtLon.Text = longitude.ToString();
            }
        }

        private void LoadRawData()
        {
            rawDataList.Items.Clear();
            GetRawData(rawData);
        }

        private void lblRaw_Click(object sender, EventArgs e)
        {
            LoadRawData();

        }


        public void UpdateField(string objName, string value)
        {
            dynamic ctrl;
            if (objName == "edtBio")
            {
                ctrl = this.Controls.Find(objName, true).First() as RichTextBox;
                ctrl.AppendText(Environment.NewLine + value.Replace("'","''"));
            }
            if (objName == "edtWork")
            {
                ctrl = this.Controls.Find(objName, true).First() as TextBox;
                ctrl.Text = value;
            }
            if (objName.Contains("edt") && objName != "edtBio" && objName != "edtWork")
            {
                ctrl = this.Controls.Find(objName, true).First() as TextBox;
                                 
                ctrl.Text = value.Replace("\"","");
            }
            if (objName.Contains("State"))
            {
                ctrl = this.Controls.Find(objName, true).First() as ComboBox;
                ctrl.SelectedItem = value;
            }
            if (objName.Contains("cb") && !objName.Contains("State"))
            {
                ctrl = this.Controls.Find(objName, true).First() as CheckBox;
                ctrl.Checked = true;
            }

            if (cbState.SelectedItem != "")
            {
                button1_Click(this,null);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var heel = cbHeel.Checked ? 1 : 0;
            var face = cbFace.Checked ? 1 : 0;
            var inring = cbInRing.Checked ? 1 : 0;
            var referee = cbRef.Checked ? 1 : 0;
            var backstage = cbBackstage.Checked ? 1 : 0;
            var booker = cbBooker.Checked ? 1 : 0;
            var announcer = cbAnnouncer.Checked ? 1 : 0;
            var color = cbColor.Checked ? 1 : 0;
            var mgr = cbMGR.Checked ? 1 : 0;
            var oldschool = cbOldSchool.Checked ? 1 : 0;
            var brawler = cbBrawler.Checked ? 1 : 0;
            var grappler = cbGrappler.Checked ? 1 : 0;
            var flyer = cbHighFlying.Checked ? 1 : 0;
            var daredevil = cbDaredevil.Checked ? 1 : 0;
            var lucha = cbLucha.Checked ? 1 : 0;
            var strongstyle = cbStrongStyle.Checked ? 1 : 0;
            var mma = cbMMA.Checked ? 1 : 0;
            var cruiser = cbCruiserweight.Checked ? 1 : 0;
            var hardcore = cbHardcore.Checked ? 1 : 0;
            var deathmatch = cbDeathMatch.Checked ? 1 : 0;
            var leader = cbLeader.Checked ? 1 : 0;
            var prankster = cbPrank.Checked ? 1 : 0;
            var leave = cbInandOut.Checked ? 1 : 0;
            var se = cbSE.Checked ? 1 : 0;
            var drink = cbDrinks.Checked ? 1 : 0;
            var smoke = cbSmokes.Checked ? 1 : 0;
            var bully = cbOldSchool.Checked ? 1 : 0;
            var helpful = cbHelps.Checked ? 1 : 0;
            var watch = cbWatches.Checked ? 1 : 0;
            var creative = cbCreative.Checked ? 1 : 0;



            var q = new StringBuilder();
            q.Append("Update TALENT Set ");
            q.Append($"NAME = '{edtName.Text.Replace("'","''")}'");
            q.Append($",NICKNAME	= '{edtNick.Text.Replace("'", "''")}'" );
            q.Append($",BIRTHMONTH = '{edtBMth.Text}'" );
            q.Append($",BIRTHYEAR = '{edtByr.Text}'" );
            q.Append($",DEBUT_MTH = '{edtdMth.Text}'" );
            q.Append($",DEBUT_YR	= '{edtDyr.Text}'" );
            q.Append($",CITY = '{edtCity.Text.Replace("'", "''")}'" );
            q.Append($",STATE = '{cbState.SelectedItem}'" );
            q.Append($",THEMESONG = '{edtTheme.Text.Replace("'", "''")}'" );
            q.Append($",HEEL = '{heel}'" );
            q.Append($",FACE = '{face}'" );
            q.Append($",INRING = '{inring}'" );
            q.Append($",REF = '{referee}'" );
            q.Append($",BACKSTAGE = '{backstage}'" );
            q.Append($",BOOKER = '{booker}'" );
            q.Append($",ANNOUNCER = '{announcer}'" );
            q.Append($",COLOR = '{color}'" );
            q.Append($",MANAGER_VALET = '{mgr}'" );
            q.Append($",OLDSCHOOL = '{oldschool}'" );
            q.Append($",BRAWLER = '{brawler}'" );
            q.Append($",GRAPPLER	= '{grappler}'" );
            q.Append($",HIGHFLYING = '{flyer}'" );
            q.Append($",DAREDEVIL = '{daredevil}'" );
            q.Append($",LUCHA = '{lucha}'" );
            q.Append($",STRONGSTYLE = '{strongstyle}'" );
            q.Append($",MMA = '{mma}'" );
            q.Append($",CRUISERWEIGHT = '{cruiser}'" );
            q.Append($",HARDCORE	= '{hardcore}'" );
            q.Append($",DEATHMATCH	= '{deathmatch}'" );
            q.Append($",MOVESLIST = '{edtID.Text}'" );
            q.Append($",LEADER = '{leader}'" );
            q.Append($",PRANKSTER = '{prankster}'" );
            q.Append($",INANDOUT = '{leave}'" );
            q.Append($",STRAIGHTEDGE	= '{se}'" );
            q.Append($",DRINK = '{drink}'" );
            q.Append($",SMOKE = '{smoke}'" );
            q.Append($",BULLY = '{bully}'" );
            q.Append($",HELPFUL = '{helpful}'" );
            q.Append($",WATCHOTHERS = '{watch}'" );
            q.Append($",CREATIVECONTROL = '{creative}'" );
            var bioClean = $"{edtID.Text.Replace("'", "''").Replace("’", "''").Replace("\'", "''")}";
            q.Append($",BIO_ID = '{bioClean}'" );           
            q.Append($",CurrentLat = '{edtLat.Text}'" );
            q.Append($",CurrentLong = '{edtLon.Text}'" );
            q.Append($",HomeLat = '{edtLat.Text}'" );
            q.Append($",HomeLong	= '{edtLon.Text}'" );
            q.Append($",Twitter = '{edtTW.Text}'");
            q.Append($",Instagram = '{edtIG.Text}'");
            q.Append($",Facebook = '{edtFB.Text}'");
            q.Append($",Youtube = '{edtYT.Text}'");
            q.Append($",TIKTOK = '{edtTT.Text}'");
            q.Append($",TWITCH = '{edtTWC.Text}'");
            q.Append($",URL = '{edtURL.Text}'");
            q.Append($" where TALENT_ID = {edtID.Text}");
            IndyPro22DatabaseManagementTool.DB.UpdateOrInsert(q.ToString());

            // Now insert the bio
            IndyPro22DatabaseManagementTool.DB.UpdateOrInsert($"Insert into TalentBio (ID, BIO) Values ('{edtID.Text}','{edtBio.Text}')");
            var selectedIndex = listView1.SelectedItems[0].Index;
            listView1.Items.RemoveAt(selectedIndex);
            listView1.Items[selectedIndex].Selected = true;
            listView1.Select();
            edtWork.Clear();

        }

        private void SaveSMOnly()
        {
            var q = new StringBuilder();
            q.Append("Update TALENT Set ");
            q.Append($"NAME = '{edtName.Text.Replace("'", "''")}'");
            q.Append($",Twitter = '{edtTW.Text}'");
            q.Append($",Instagram = '{edtIG.Text}'");
            q.Append($",Facebook = '{edtFB.Text}'");
            q.Append($",Youtube = '{edtYT.Text}'");
            q.Append($",TIKTOK = '{edtTT.Text}'");
            q.Append($",TWITCH = '{edtTWC.Text}'");
            q.Append($",URL = '{edtURL.Text}'");
            q.Append($" where TALENT_ID = {edtID.Text}");
            IndyPro22DatabaseManagementTool.DB.UpdateOrInsert(q.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.RemoveAt(listView1.SelectedItems[0].Index);
            listView1.Items[0].Selected = true;
            listView1.Select();

        }

        public string GetDBName()
        {
            return edtWork.Text;
        }

        // Handle Raw Data
        public void GetRawData(DataTable rawData)
        {
            foreach (DataColumn column in rawData.Columns)
            {
                var row = rawData.Rows[0];
                rawDataList.Items.Add(DB.GetStrData(row, column.ColumnName));

            }
        }

        public async void GeocodeAsync(string address)
        {
            string Address = string.Empty;
            var req = new GeocodeRequest();
            req.BingMapsKey = "AvrksM67bdtn2IAeV-Ovkm2DwWbYaNPa6xucbqQBVK8o081MQCPUF4MYvEDfpnD0";

            // Set the address
            req.Query = address;

            // Make the geocode request
            var result = await req.Execute();
            if (result.StatusCode == 200)
            {
                var toolkitLocation = (result?.ResourceSets?.FirstOrDefault())
               ?.Resources?.FirstOrDefault()
               as BingMapsRESTToolkit.Location;
                Address = toolkitLocation.Address.FormattedAddress;

            }
            List<string> citySt = new List<string>();
            var City = string.Empty;
            var State = string.Empty;

            if (Address.Contains(","))
            {
                citySt = Address.Split(',').ToList();
                City = citySt[0].Trim();
                State = citySt[1].Trim();
            }
            else
            {

            }

            UpdateField("edtCity", City);
            UpdateField("cbState", State);

        }

        private async void rawDataList_Click(object sender, EventArgs e)
        {
            string[] monthAbbrev = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;

            var colSelected = ColList.Items[rawDataList.SelectedIndex].ToString();
            var data = rawDataList.Items[rawDataList.SelectedIndex].ToString();
            if (colSelected == "Links")
            {
                UpdateField("edtWork", data);
              
            }
            if (colSelected.Contains("edt"))
            {              
                if (colSelected == "edtBMth")
                {
                    data = data.Replace("nd", "").Replace("th", "").Replace("st", "").Replace("rd", "");
                    //parse the date and post both
                    List<string> date = data.Split(' ').ToList();
                    dynamic mth = 1;
                    dynamic year = 1900;
                    bool successfullyParsed = int.TryParse(date[0], out int ignoreMe);
                    if (successfullyParsed)
                    {
                        mth = date[0];
                        year = date[2];
                    }
                    else
                    {                        
                        //check if it is in date format
                        var d1 = DateTime.TryParse(data, out DateTime newDate);
                        if (d1)
                        {

                            mth = newDate.Month;
                            year = newDate.Year;
                        }
                        else if (data.IndexOf('-', StringComparison.Ordinal) > 0 && data.Any(x => char.IsLetter(x)))
                        {
                            date = data.Split('-').ToList();  
                           
                           var dateFormat1 = date[0];
                           var isFullMonthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames.Contains(dateFormat1);
                           var isAbbMonthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames.Contains(dateFormat1);
                           if (isFullMonthname || isAbbMonthname)
                           {
                               // Handle the first part being
                               mth = dateFormat1;
                               if (isAbbMonthname)
                                    mth =   Array.IndexOf(monthAbbrev, mth) + 1;
                               else
                                    mth = DateTime.ParseExact(date[0], "MMMM", CultureInfo.CurrentCulture).Month;

                            }
                           var dateFormat2 = date[1];
                           isFullMonthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames.Contains(dateFormat2);
                           isAbbMonthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames.Contains(dateFormat2);
                           if (isFullMonthname || isAbbMonthname)
                           {
                               // Handle the first part being
                               mth = dateFormat2;
                                if (isAbbMonthname)
                                    mth = Array.IndexOf(monthAbbrev, mth) + 1;
                                else
                                    mth = DateTime.ParseExact(date[0], "MMMM", CultureInfo.CurrentCulture).Month;

                            }
                            var ValidYear = int.TryParse(date[0], out int newDateYR);
                            if (ValidYear)
                            {                               
                                year = int.Parse(date[0]);
                            }
                            ValidYear = int.TryParse(date[1], out int newDateYR2);
                            if (ValidYear)
                            {                              
                                year = int.Parse(date[1]);
                            }
                            else 
                                year = "2020";                          
                          
                        }
                        else
                        {
                            mth = DateTime.ParseExact(date[0], "MMMM", CultureInfo.CurrentCulture).Month;
                            year = date[2];
                        }                       

                       var finalBirthDate = new DateTime(year, mth,1);
                       mth = finalBirthDate.Month;
                       year = finalBirthDate.Year;
                        if (year < 100)
                            year = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(year);


                    }                   
                    UpdateField("edtBMth", mth.ToString());
                    UpdateField("edtByr", year.ToString());
                }
                else if (colSelected == "edtdMth")
                {
                    data = data.Replace("nd", "").Replace("th", "").Replace("st", "").Replace("rd","");
                    var ValidYear = int.TryParse(data, out int newDateYR);
                    if (ValidYear)
                    {
                        if (newDateYR > 1970 && newDateYR < 2022)
                            data = $"01/01/{data}";
                    }

                    List<string> date = data.Split(' ').ToList();
                    dynamic mth = 1;
                    dynamic year = 1900;
                    bool successfullyParsed = int.TryParse(date[0], out int ignoreMe);
                    if (successfullyParsed)
                    {
                        mth = date[0];
                        year = date[2];
                    }
                    else
                    {
                        var d1 = DateTime.TryParse(data, out DateTime newDate);
                        if (d1)
                        {

                            mth = newDate.Month;
                            year = newDate.Year;
                            if (year == 2022)
                                year = 2020;
                        }
                        else if (data.IndexOf('-', StringComparison.Ordinal) > 0 && data.Any(x => char.IsLetter(x)))
                        {
                            date = data.Split('-').ToList();

                            var dateFormat1 = date[0];
                            var isFullMonthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames.Contains(dateFormat1);
                            var isAbbMonthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames.Contains(dateFormat1);
                            if (isFullMonthname || isAbbMonthname)
                            {
                                // Handle the first part being
                                mth = dateFormat1;
                                if (isAbbMonthname)
                                    mth = Array.IndexOf(monthAbbrev, mth) + 1;
                                else
                                    mth = DateTime.ParseExact(date[0], "MMMM", CultureInfo.CurrentCulture).Month;
                            }
                            var dateFormat2 = date[1];
                            isFullMonthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames.Contains(dateFormat2);
                            isAbbMonthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames.Contains(dateFormat2);
                            if (isFullMonthname || isAbbMonthname)
                            {
                                // Handle the first part being
                                mth = dateFormat2;
                                if (isAbbMonthname)
                                    mth = Array.IndexOf(monthAbbrev, mth) + 1;
                                else
                                    mth = DateTime.ParseExact(date[0], "MMMM", CultureInfo.CurrentCulture).Month;
                            }
                            ValidYear = int.TryParse(date[0], out int newDateYRd);
                            if (ValidYear)
                            {
                                year = int.Parse(date[0]);
                            }
                            ValidYear = int.TryParse(date[1], out int newDateYRd2);
                            if (ValidYear)
                            {
                                year = int.Parse(date[1]);
                            }
                            else
                                year = "2020";


                        }
                        else
                        {
                            mth = DateTime.ParseExact(date[0], "MMMM", CultureInfo.CurrentCulture).Month;
                            year = date[2];
                        }
                        var finalBirthDate = new DateTime(year, mth, 1);
                        mth = finalBirthDate.Month;
                        year = finalBirthDate.Year;
                        if (year < 100)
                            year = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(year);
                    }
                   

                   UpdateField("edtdMth", mth.ToString());
                   UpdateField("edtDyr", year.ToString());
                }
                else if (colSelected == "edtCity")
                {
                    GeocodeAsync(data);

                }
                else
                   UpdateField(colSelected, data);
            }
            if (colSelected == "Roles")
            {
                if (data.Contains("In-ring"))
                {
                    UpdateField("cbInRing", data);
                }
                if (data.Contains("Backstage"))
                {
                    UpdateField("cbBackstage", data);
                }
                if (data.Contains("Manager"))
                {
                    UpdateField("cbMGR", data);
                }
                if (data.Contains("Color"))
                {
                    UpdateField("cbColor", data);
                }
                if (data.Contains("Announcing") || data.Contains("Announcer"))
                {
                    UpdateField("cbAnnouncer", data);
                }
                if (data.Contains("Booker"))
                {
                    UpdateField("cbBooker", data);
                }

            }
            if (colSelected == "HorF")
            {
                if (data.Contains("Heel"))
                {
                    UpdateField("cbHeel", data);
                }
                if (data.Contains("Face"))
                {
                    UpdateField("cbFace", data);
                }
                if (data.Contains("Both"))
                {
                    UpdateField("cbHeel", data);
                    UpdateField("cbFace", data);
                }

            }
            if (colSelected == "Styles")
            {
                if (data.Contains("Old School"))
                {
                    UpdateField("cbOldSchool", data);
                }
                if (data.Contains("Brawler"))
                {
                    UpdateField("cbBrawler", data);
                }
                if (data.Contains("Grappler"))
                {
                    UpdateField("cbGrappler", data);
                }
                if (data.Contains("Flyer"))
                {
                    UpdateField("cbHighFlying", data);
                }
                if (data.Contains("Daredevil"))
                {
                    UpdateField("cbDaredevil", data);
                }
                if (data.Contains("Lucha"))
                {
                    UpdateField("cbLucha", data);
                }
                if (data.Contains("Strong Style"))
                {
                    UpdateField("cbStrongStyle", data);
                }
                if (data.Contains("MMA"))
                {
                    UpdateField("cbMMA", data);
                }
                if (data.Contains("Cruiserweight"))
                {
                    UpdateField("cbCruiserweight", data);
                }
                if (data.Contains("Hardcore"))
                {
                    UpdateField("cbHardcore", data);
                }
                if (data.Contains("Death"))
                {
                    UpdateField("cbDeathMatch", data);
                }
            }          
            if (colSelected == "Personality")
            {
                if (data.Contains("Leader"))
                {
                    UpdateField("cbLeader", data);
                }
                if (data.Contains("Prankster"))
                {
                    UpdateField("cbPrank", data);
                }
                if (data.Contains("Drink"))
                {
                    UpdateField("cbDrinks", data);
                }
                if (data.Contains("Smoke"))
                {
                    UpdateField("cbSmokes", data);
                }
                if (data.Contains("help"))
                {
                    UpdateField("cbHelps", data);
                }
                if (data.Contains("watch"))
                {
                    UpdateField("cbWatches", data);
                }
                if (data.Contains("involved"))
                {
                    UpdateField("cbCreative", data);
                }
                if (data.Contains("Bully"))
                {
                    UpdateField("cbBully", data);
                }
                if (data.Contains("Straight Edge") && (!data.Contains("Smoke") || !data.Contains("Drink")))
                {
                    UpdateField("cbSE", data);
                }
                if (data.Contains("leave"))
                {
                    UpdateField("cbInandOut", data);
                }

            }

        }        

        public void GetFilePath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = $"C:\\Users\\jwest\\OneDrive\\Documents\\GitHub\\IndyWrestling\\Assets\\StreamingAssets\\MamasHomeCooking\\DONOTDELETE";
                openFileDialog.Filter = "DB files (*.ip2)|*.ip2|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    DB.SetDb($"URI=file:{filePath}");
                }
            }
        }

        private void btnGetDB_Click(object sender, EventArgs e)
        {
           GetFilePath();
        }

        private void Navigate()
        {
            if (addressBar.Text == "") return;
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(addressBar.Text);
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            Navigate();
        }

        private void addressBar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                Navigate();
            }
        }

        private void btnFB_Click(object sender, EventArgs e)
        {
            currentSM = SocialMedia.FaceBook;
            addressBar.Text = $"https://www.facebook.com/{edtWork.SelectedText}";
        }

        private void webView_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            addressBar.Text = webView.Source.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            switch (currentSM)
            {
                case SocialMedia.FaceBook:
                    edtFB.Text = addressBar.Text;
                    break;
                case SocialMedia.Twitter:
                    edtTW.Text = addressBar.Text;
                    break;
                case SocialMedia.Twitch:
                    edtTWC.Text = addressBar.Text;
                    break;
                case SocialMedia.TikTok:
                    edtTT.Text = addressBar.Text;
                    break;
                case SocialMedia.Instgram:
                    edtIG.Text = addressBar.Text;
                    break;
                case SocialMedia.URL:
                    edtURL.Text = addressBar.Text;
                    break;
                case SocialMedia.YouTube:
                    edtYT.Text = addressBar.Text;
                    break;
            }
        }

        private void btnTW_Click(object sender, EventArgs e)
        {
            currentSM = SocialMedia.Twitter;
            addressBar.Text = $"https://www.twitter.com/{edtWork.SelectedText.Replace("@","")}";
        }

        private void btnIG_Click(object sender, EventArgs e)
        {
            currentSM = SocialMedia.Instgram;
            addressBar.Text = $"https://www.Instagram.com/{edtWork.SelectedText.Replace("@", "")}";
        }

        private void btnTT_Click(object sender, EventArgs e)
        {
            currentSM = SocialMedia.TikTok;
            addressBar.Text = $"https://www.TikTok.com/{edtWork.SelectedText.Replace("@", "")}";
        }

        private void btnYT_Click(object sender, EventArgs e)
        {
            currentSM = SocialMedia.YouTube;
            addressBar.Text = $"https://www.youtube.com/{edtWork.SelectedText.Replace("@", "")}";
        }

        private void btnTwitch_Click(object sender, EventArgs e)
        {
            currentSM = SocialMedia.Twitch;
            addressBar.Text = $"https://www.twitch.tv/{edtWork.SelectedText.Replace("@", "")}";
        }

        private void btnURL_Click(object sender, EventArgs e)
        {
            currentSM = SocialMedia.URL;
            addressBar.Text = $"https://{edtWork.SelectedText.Replace("@", "")}";
        }

        private void addressBar_TextChanged(object sender, EventArgs e)
        {
            Navigate();
        }

        private void cbState_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbState.SelectedIndex == 0) return;
            GetLatLong();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveSMOnly();
        }
    }
}