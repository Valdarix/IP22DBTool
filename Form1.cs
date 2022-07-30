using System.Data;
using Microsoft.VisualBasic.FileIO;
using BingMapsRESTToolkit;
using System.Text;
using System.Globalization;

namespace IndyPro22DatabaseManagementTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

            
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    listView1.Clear();
                    var reader = DB.GetDataReader($"Select * from Talent where Talent_ID NOT IN (Select ID FROM TalentBio) order by Name COLLATE NOCASE Asc ");                  
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
            rawDataList.Items.Clear();
            edtBio.Clear();
            edtLat.Clear();
            edtLon.Clear();
            if (listView1.SelectedItems.Count == 0)
                return;
            var selected = listView1.SelectedItems[0].Text;
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
                ctrl.AppendText(Environment.NewLine + value);
            }
            if (objName == "edtWork")
            {
                ctrl = this.Controls.Find(objName, true).First() as TextBox;
                ctrl.Text = value;
            }
            if (objName.Contains("edt") && objName != "edtBio" && objName != "edtWork")
            {
                ctrl = this.Controls.Find(objName, true).First() as TextBox;
              
                    
                ctrl.Text = value;
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
            q.Append($"NICKNAME	= '{edtNick.Text.Replace("'", "''")}'" );
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
            q.Append($" where TALENT_ID = {edtID.Text}");
            IndyPro22DatabaseManagementTool.DB.UpdateOrInsert(q.ToString());

            // Now insert the bio
            IndyPro22DatabaseManagementTool.DB.UpdateOrInsert($"Insert into TalentBio (ID, BIO) Values ('{edtID.Text}','{edtBio.Text}')");
            var selectedIndex = listView1.SelectedItems[0].Index;
            listView1.Items.RemoveAt(selectedIndex);
            listView1.Items[selectedIndex].Selected = true;
            listView1.Select();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.RemoveAt(listView1.SelectedItems[0].Index);
            listView1.Items[0].Selected = true;
            listView1.Select();

        }

        public string GetDBName()
        {
            return edtWrok.Text;
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
                    //parse the date and post both
                    List<string> date = data.Split(' ').ToList();
                    dynamic mth;
                    dynamic year;
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
                        else// must be in text format
                        {
                            mth = DateTime.ParseExact(date[0], "MMMM", CultureInfo.CurrentCulture).Month;
                            year = date[2];
                        }

                    }

                    UpdateField(colSelected, mth.ToString());
                    UpdateField("edtByr", year.ToString());
                }
                else if (colSelected == "edtdMth")
                {
                    var ValidYear = int.TryParse(data, out int newDateYR);
                    if (ValidYear)
                    {
                        if (newDateYR > 1970 && newDateYR < 2022)
                            data = $"01/01/{data}";
                    }

                    List<string> date = data.Split(' ').ToList();
                    dynamic mth;
                    dynamic year;
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
                        }
                        else// must be in text format
                        {
                            mth = DateTime.ParseExact(date[0], "MMMM", CultureInfo.CurrentCulture).Month;
                            year = date[2];
                        }
                    }

                   UpdateField(colSelected, mth.ToString());
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
    }
}