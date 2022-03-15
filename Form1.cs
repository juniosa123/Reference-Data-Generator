using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using wedoIT.CATS.Tools.ReferenceDataGenerator.Helper;
using wedoIT.CATS.Tools.ReferenceDataGenerator.Model;
using wedoIT.CATS.Tools.ReferenceDataGenerator.Parser;

namespace wedoIT.CATS.Tools.ReferenceDataGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateInput();
                txtResult.Text = GenerateRefDataText();
                //saveFileDialog1.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ValidateInput()
        {
            if (txtRefDataType.Text.Trim().Length == 0 || txtCode.Text.Trim().Length == 0 || txtCodeName.Text.Trim().Length == 0 || txtDescEN.Text.Trim().Length == 0 || txtDescID.Text.Trim().Length == 0)
                throw new Exception("Please fill in the mandatory field");
        }

        public string GenerateRefDataText()
        {
            ReferenceData data = MappingReferenceData();
            StringBuilder sb = new StringBuilder();
            WriteText(data, sb);
            return sb.ToString();
        }

        private ReferenceData MappingReferenceData()
        {
            ReferenceData data = new ReferenceData();
            data.Aggregates.AggregateIdentifier = GuidParser.GuidToRaw(System.Guid.NewGuid());
            data.Events.AggregateIdentifier = data.Aggregates.AggregateIdentifier;
            data.Maintenances.RecordId = GuidParser.GuidToRaw(System.Guid.NewGuid());
            data.Events.EventData = GenerateEventData(data.Maintenances.RecordId);
            data.Maintenances.AggregateIdentifier = data.Aggregates.AggregateIdentifier;
            data.Maintenances.ReferenceDataType = txtRefDataType.Text.Trim().ToUpper();
            data.Maintenances.Code = txtCode.Text.Trim();
            data.Maintenances.ParameterData = txtParameterData.Text.Trim();
            if (data.Maintenances.ParameterData.Length == 0)
                data.Maintenances.ParameterData = "{}";
            data.Maintenances.Localization = GenerateMaintenanceLocalization();

            ReferenceDataCurrents currEN = new ReferenceDataCurrents();
            currEN.ReferenceDataType = txtRefDataType.Text.Trim().ToUpper();
            currEN.Code = txtCode.Text.Trim();
            currEN.CodeName = txtCodeName.Text.Trim();
            currEN.Code_Description = txtDescEN.Text.Trim();
            currEN.AppLanguageId = "en-US";
            currEN.ParameterData = txtParameterData.Text.Trim();

            if (currEN.ParameterData.Length == 0)
                currEN.ParameterData = "{}";

            data.ListCurrent.Add(currEN);

            ReferenceDataCurrents currID = new ReferenceDataCurrents();
            currID.ReferenceDataType = txtRefDataType.Text.Trim().ToUpper();
            currID.Code = txtCode.Text.Trim();
            currID.CodeName = txtCodeName.Text.Trim() + " (ID)";
            currID.Code_Description = txtDescID.Text.Trim();
            currID.AppLanguageId = "id-ID";
            currID.ParameterData = txtParameterData.Text.Trim();
            if (currID.ParameterData.Length == 0)
                currID.ParameterData = "{}";
            data.ListCurrent.Add(currID);

            return data;
        }

        private string GenerateMaintenanceLocalization()
        {
            return "[{\"AppLanguageId\":\"id-ID\",\"CodeName\":\"" + txtCodeName.Text.Trim() + " (ID)\",\"CodeDescription\":\"" + txtDescID.Text.Trim() + "\"},{\"AppLanguageId\":\"en-US\",\"CodeName\":\"" + txtCodeName.Text.Trim() + "\",\"CodeDescription\":\"" + txtDescEN.Text.Trim() + "\"}]";
        }

        private string GenerateMaintenanceLocalization(string code, string codeName, string descID, string descEN)
        {
            return "[{\"AppLanguageId\":\"id-ID\",\"CodeName\":\"" + codeName.Trim() + " (ID)\",\"CodeDescription\":\"" + descID.Trim() + "\"},{\"AppLanguageId\":\"en-US\",\"CodeName\":\"" + codeName.Trim() + "\",\"CodeDescription\":\"" + descEN.Trim() + "\"}]";
        }

        private string GenerateEventData(string recordId)
        {
            return "{\"RecordId\":\"" + recordId + "\",\"ReferenceDataType\":\"" + txtRefDataType.Text.Trim().ToUpper() + "\",\"Code\":\"" + txtCodeName.Text.Trim() + "\",\"ParameterData\":{\"ValueKind\":1},\"ValidFrom\":\"2008-01-01T00:00:00\",\"ValidTo\":null,\"Localization\":[{\"AppLanguageId\":\"id-ID\",\"CodeName\":\"" + txtCodeName.Text.Trim() + " (ID)\",\"CodeDescription\":\"" + txtDescID.Text.Trim() + "\"},{\"AppLanguageId\":\"en-US\",\"CodeName\":\"" + txtCodeName.Text.Trim() + "\",\"CodeDescription\":\"" + txtDescEN.Text.Trim() + "\"}]}";
        }

        private string GenerateEventData(string recordId, string refDataType, string code, string codeName, string descID, string descEN)
        {
            return "{\"RecordId\":\"" + recordId + "\",\"ReferenceDataType\":\"" + refDataType.Trim().ToUpper() + "\",\"Code\":\"" + code.Trim() + "\",\"ParameterData\":{\"ValueKind\":1},\"ValidFrom\":\"2008-01-01T00:00:00\",\"ValidTo\":null,\"Localization\":[{\"AppLanguageId\":\"id-ID\",\"CodeName\":\"" + codeName.Trim() + " (ID)\",\"CodeDescription\":\"" + descID.Trim() + "\"},{\"AppLanguageId\":\"en-US\",\"CodeName\":\"" + codeName.Trim() + "\",\"CodeDescription\":\"" + descEN.Trim() + "\"}]}";
        }

        private void WriteText(ReferenceData data, StringBuilder sb)
        {
            AppendAggregates(data.Aggregates, sb);
            sb.AppendLine();
            AppendEvents(data.Events, sb);
            sb.AppendLine();
            AppendMaintenance(data.Maintenances, sb);
            sb.AppendLine();
            AppendCurrents(data.ListCurrent, sb);
        }

        private void WriteTextBulk(List<ReferenceData> data, StringBuilder sb)
        {
            try
            {
                foreach (var refData in data)
                {
                    AppendAggregates(refData.Aggregates, sb);
                }

                sb.AppendLine();

                foreach (var refData in data)
                {
                    AppendEvents(refData.Events, sb);
                }

                sb.AppendLine();

                foreach (var refData in data)
                {
                    AppendMaintenance(refData.Maintenances, sb);
                }

                sb.AppendLine();

                foreach (var refData in data)
                {
                    AppendCurrents(refData.ListCurrent, sb);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while writing the txt file : " + ex.Message);
            }



        }

        private void AppendCurrents(List<ReferenceDataCurrents> listCurrent, StringBuilder sb)
        {
            foreach (var data in listCurrent)
            {
                var text = @"INSERT INTO reference_data.current_reference_data (Reference_Data_Type, Code, Code_Name, Code_Description, App_Language_Id, Valid_To, Parameter_Data) VALUES ('" + data.ReferenceDataType + "', '" + data.Code + "', '" + data.CodeName + "', '" + data.Code_Description + "', '" + data.AppLanguageId + "', NULL, '" + data.ParameterData + "');";
                sb.AppendLine(text);
            }
        }

        private void AppendMaintenance(ReferenceDataMaintenance data, StringBuilder sb)
        {
            var text = @"INSERT INTO reference_data.maintenance_reference_data (Record_Id, Aggregate_Identifier, Aggregate_Version, Reference_Data_Type, Code, Parameter_Data, Localization, Valid_From, Valid_To) VALUES ('" + data.RecordId + "', '" + data.AggregateIdentifier + "', " + data.AggregateVersion + ", '" + data.ReferenceDataType + "', '" + data.Code + "', '" + data.ParameterData + "', '" + data.Localization + "', '" + data.ValidFrom + "', " + data.ValidTo + ");";
            sb.AppendLine(text);
        }

        private void AppendAggregates(ReferenceDataAggregates data, StringBuilder sb)
        {
            var text = @"INSERT INTO reference_data.aggregates (Aggregate_Identifier, Aggregate_Class, Aggregate_Type, Tenant_Identifier, Aggregate_Expires) VALUES ('" + data.AggregateIdentifier + "'" + ",'" + data.AggregateClass + "'" + ",'" + data.AggregateType + "'" + ",'" + data.TenantIdentifier + "'" + "," + data.AggregateExpires + ");";
            sb.AppendLine(text);
        }

        private void AppendEvents(ReferenceDataEvents data, StringBuilder sb)
        {
            var text = @"INSERT INTO reference_data.events (Aggregate_Identifier, Aggregate_Version, Identity_Tenant, Identity_User, Event_Class, Event_Type, Event_Data, Event_Time) VALUES ('" + data.AggregateIdentifier + "', " + data.AggregateVersion + ", '" + data.IdentityTenant + "', '" + data.IdentityUser + "', '" + data.EventClass + "', '" + data.EventType + "', '" + data.EventData + "', " + data.EventTime + ");";
            sb.AppendLine(text);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            GenerateRefDataText();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtResult.Text.Trim());
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                ExcelHelper helper = new ExcelHelper();
                helper.CreateExcelFile(documentPath);
                MessageBox.Show("File is located on " + documentPath + "\\RefDataTemplate.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                openFileDialog1.Multiselect = false;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnProcessBulk_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFilePath.Text.Trim().Length == 0)
                    throw new Exception("Please select the file");

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    sfd.FilterIndex = 1;
                    sfd.FileName = "GenerateReferenceData.txt";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        var dsResult = ExcelParser.ParseExcel(txtFilePath.Text.Trim());
                        var listToProcess = MappingReferenceDataFromDataset(dsResult);


                        StringBuilder sb = new StringBuilder();
                        WriteTextBulk(listToProcess, sb);
                        File.WriteAllText(sfd.FileName, sb.ToString());

                        if (chkCreateEnumFile.Checked)
                        {
                            var path = System.IO.Path.GetDirectoryName(sfd.FileName);
                            WriteEnumFile(listToProcess, path);
                        }

                        MessageBox.Show("Reference Data are generated");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<ReferenceData> MappingReferenceDataFromDataset(DataSet ds)
        {
            List<ReferenceData> listData = new List<ReferenceData>();
            try
            {
                var datatable = ds.Tables[0];

                for (int i = 0; i < datatable.Rows.Count - 1; i++)
                {
                    string refDataType = datatable.Rows[i + 1][(int)dataType.Value].ToString().Trim().Replace("'", "").Replace("\"", "");
                    string code = datatable.Rows[i + 1][(int)codeColumn.Value].ToString().Trim().Replace("'", "").Replace("\"", "");
                    string codeName = datatable.Rows[i + 1][(int)codeNameColumn.Value].ToString().Trim().Replace("'", "").Replace("\"", "");
                    string parameterData = datatable.Rows[i + 1][(int)parameter.Value].ToString().Trim().Replace("'", "");
                    string descEN = datatable.Rows[i + 1][(int)descEng.Value].ToString().Trim().Replace("'", "").Replace("\"", "");
                    string descID = datatable.Rows[i + 1][(int)descInd.Value].ToString().Trim().Replace("'", "").Replace("\"", "");

                    ReferenceData data = new ReferenceData();
                    data.Aggregates.AggregateIdentifier = GuidParser.GuidToRaw(System.Guid.NewGuid());
                    data.Events.AggregateIdentifier = data.Aggregates.AggregateIdentifier;
                    data.Maintenances.RecordId = GuidParser.GuidToRaw(System.Guid.NewGuid());
                    data.Events.EventData = GenerateEventData(data.Maintenances.RecordId, refDataType, code, codeName, descID, descEN);
                    data.Maintenances.AggregateIdentifier = data.Aggregates.AggregateIdentifier;
                    data.Maintenances.ReferenceDataType = refDataType.Trim().ToUpper();
                    data.Maintenances.Code = code.Trim();
                    data.Maintenances.ParameterData = parameterData.Trim();
                    if (data.Maintenances.ParameterData.Length == 0)
                        data.Maintenances.ParameterData = "{}";
                    data.Maintenances.Localization = GenerateMaintenanceLocalization(code, codeName, descID, descEN);

                    ReferenceDataCurrents currEN = new ReferenceDataCurrents();
                    currEN.ReferenceDataType = refDataType.Trim().ToUpper();
                    currEN.Code = code.Trim();
                    currEN.CodeName = codeName.Trim();
                    currEN.Code_Description = descEN.Trim();
                    currEN.AppLanguageId = "en-US";
                    currEN.ParameterData = parameterData.Trim();

                    if (currEN.ParameterData.Length == 0)
                        currEN.ParameterData = "{}";

                    data.ListCurrent.Add(currEN);

                    ReferenceDataCurrents currID = new ReferenceDataCurrents();
                    currID.ReferenceDataType = refDataType.Trim().ToUpper();
                    currID.Code = code.Trim();
                    currID.CodeName = codeName.Trim() + " (ID)";
                    currID.Code_Description = descID.Trim();
                    currID.AppLanguageId = "id-ID";
                    currID.ParameterData = parameterData.Trim();
                    if (currID.ParameterData.Length == 0)
                        currID.ParameterData = "{}";
                    data.ListCurrent.Add(currID);

                    listData.Add(data);

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while mapping variable from excel file : " + ex.Message);
            }



            return listData;
        }

        private void txtFilePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void WriteEnumFile(List<ReferenceData> listToProcess, string path)
        {
            var listCurrent = listToProcess.SelectMany(x => x.ListCurrent).ToList();
            var listRefDataType = listCurrent.Select(x => x.ReferenceDataType).Distinct().ToList();
            foreach (var item in listRefDataType)
            {
                var listCurrentPerType = listCurrent.Where(x => x.ReferenceDataType.Equals(item)).ToList();
                WriteEnumFilePerType(item, path, listCurrentPerType);
            }
        }

        private void WriteEnumFilePerType(string refDataType, string path, List<ReferenceDataCurrents> listCurrentPerType)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var ParameterDataName = textInfo.ToTitleCase(refDataType.Replace("_", " ").ToLower()).Replace(" ", string.Empty);
            var EnumName = ParameterDataName + "Enum";
            var FileName = path + "\\" + EnumName + ".cs";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using wedoIT.CATS.Shared.ReferenceData.Base;");
            sb.AppendLine();
            sb.AppendLine("namespace wedoIT.CATS.Shared.ReferenceData");
            sb.AppendLine("{");
            sb.AppendLine("\t public class " + EnumName + " : BaseReferenceDataTypeEnum");
            sb.AppendLine("\t {");
            sb.AppendLine("\t \t public const string ReferenceDataTypeName = \"" + refDataType + "\";");
            sb.AppendLine("\t \t public const string ParameterDataName = \"" + ParameterDataName + "\";");
            sb.AppendLine("\t \t private " + EnumName + "(string code) : base(code) { }");

            var listDistinctCode = listCurrentPerType.Select(x => x.Code).Distinct().ToList();

            foreach (var item in listDistinctCode)
            {
                sb.AppendLine("\t \t public static " + EnumName + " " + textInfo.ToTitleCase(listCurrentPerType.FirstOrDefault(x => x.Code == item).CodeName.ToLower()) + " => new(\"" + item + "\");");
            }

            sb.AppendLine("\t }");
            sb.AppendLine("}");
            File.WriteAllText(FileName, sb.ToString());
        }
    }
}
