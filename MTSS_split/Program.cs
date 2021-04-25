using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MTSS_split
{
    class Program
    {
        public class SetInformation
        {
            // Auto-implemented properties.
            public string JointName { get; set; }
            public int Xmin { get; set; }
            public int Ymin { get; set; }
            public int Xmax { get; set; }
            public int Ymax { get; set; }
            public SetInformation() { }
        }

        static void Main(string[] args)
        {


            //讀取路徑下所有"*JSN.xml"的Files
            DirectoryInfo di = new DirectoryInfo(@"D:\LAB\mtss\mTss-file\mTSS-927_png+ROI\");
            var DICOM = di.GetFiles("*JSN.xml", SearchOption.AllDirectories);
            List<string> ErrorFile = new List<string>();
            foreach (var XmlFileName in DICOM)
            {
                try
                {
                    Console.WriteLine("檔案：" + XmlFileName.FullName + "  檔名：" + XmlFileName); //print
                    //載入XML
                    XDocument xDoc = XDocument.Load(XmlFileName.FullName); //XDocument 讀XML檔的函式
                    XElement xElement = xDoc.Element("annotation"); // XELement 讀節點用的

                    if (xElement == null)
                    {
                        return;
                    }

                    XElement pathElement = xElement.Element("path");
                    var PNGPath = pathElement;
                    Console.WriteLine("PNGPath：" + PNGPath.Value);

                    //找到X.png前的路徑，替換成.Dcm的路徑
                    string PNGName = PNGPath.Value.Substring(PNGPath.Value.LastIndexOf("\\") + 1);
                    string DCMPath = XmlFileName.FullName.Replace("mTSS-927_png+ROI", "mTSS-927_U"); //後面的替換到前面
                    string DesPath = XmlFileName.FullName.Substring(0, XmlFileName.FullName.LastIndexOf("\\")) + "\\";
                    DCMPath = DCMPath.Substring(0, DCMPath.LastIndexOf("\\") + 1);
                    DCMPath = DCMPath + PNGName;
                    DCMPath = DCMPath.Replace(".png", ".dcm");

                    Console.WriteLine("PNGName：" + PNGName + "  DicomPath：" + DCMPath);

                    //-------------------------------------------------------------------
                    //讀XML－>List
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(XmlFileName.FullName);
                    List<SetInformation> infor = new List<SetInformation>();
                    XmlNodeList list = xdoc.SelectNodes("//annotation/object");
                    XmlNodeList listInner = xdoc.SelectNodes("//annotation/object/bndbox");

                    int count = 0;

                    foreach (XmlNode item in list)
                    {
                        SetInformation num1 = new SetInformation();
                        num1.JointName = item["name"].InnerText;

                        var dd = listInner[count];
                        var fu = dd["xmin"].InnerText;
                        num1.Xmin = Convert.ToInt32(dd["xmin"].InnerText);
                        num1.Ymin = Convert.ToInt32(dd["ymin"].InnerText);
                        num1.Xmax = Convert.ToInt32(dd["xmax"].InnerText);
                        num1.Ymax = Convert.ToInt32(dd["ymax"].InnerText);
                        infor.Add(num1);

                        count++;
                    }

                    //建立新資料夾NewFolder
                    string NewFolder = DesPath + "WL_jpg";
                    System.IO.Directory.CreateDirectory(NewFolder);

                    foreach (SetInformation c in infor)
                    {
                        string NewJpgName = XmlFileName.Name.Replace(".xml", ".jpg");
                        string command = "/C dcmj2pnm +Ww 2026 1791 +P +oj " + DCMPath + " " + NewFolder + "\\" + NewJpgName;
                        System.Diagnostics.Process.Start("CMD.exe", command);
                    }

                    //判斷類別與是否存在目錄
                    string f_type = "";
                    NewFolder = "_xml";
                    string NFlocation = @"D:\LAB\mtss\mTss-file\";
                    String CopyDesDir = NFlocation + f_type + NewFolder;

                    if (XmlFileName.FullName.IndexOf("H", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (XmlFileName.FullName.IndexOf("JSN", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            f_type = "JSN";
                            CopyDesDir = NFlocation + f_type + NewFolder;
                            if (!(Directory.Exists(CopyDesDir)))
                            {
                                Directory.CreateDirectory(@"D:\LAB\mtss\mTss-file\" + f_type + NewFolder);
                            }
                            foreach (SetInformation c in infor)
                            {
                                //File.Move(DCMPath, NFlocation+f_type + NewFolder);
                                File.Copy(XmlFileName.FullName, Path.Combine(CopyDesDir, XmlFileName.Name));
                            }
                        }
                        
                    }

                }
                catch (Exception ex)
                {
                    ErrorFile.Add(XmlFileName.FullName);
                }
            }
            DICOM = di.GetFiles("*EROSIONS.xml", SearchOption.AllDirectories);
            foreach (var XmlFileName_e in DICOM)
            {
                try
                {
                    Console.WriteLine("檔案：" + XmlFileName_e.FullName + "  檔名：" + XmlFileName_e); //print
                    //載入XML
                    XDocument xDoc = XDocument.Load(XmlFileName_e.FullName); //XDocument 讀XML檔的函式
                    XElement xElement = xDoc.Element("annotation"); // XELement 讀節點用的

                    if (xElement == null)
                    {
                        return;
                    }

                    XElement pathElement = xElement.Element("path");
                    var PNGPath = pathElement;
                    Console.WriteLine("PNGPath：" + PNGPath.Value);

                    //找到X.png前的路徑，替換成.Dcm的路徑
                    string PNGName = PNGPath.Value.Substring(PNGPath.Value.LastIndexOf("\\") + 1);
                    string DCMPath = XmlFileName_e.FullName.Replace("mTSS-927_png+ROI", "mTSS-927_U"); //後面的替換到前面
                    string DesPath = XmlFileName_e.FullName.Substring(0, XmlFileName_e.FullName.LastIndexOf("\\")) + "\\";
                    DCMPath = DCMPath.Substring(0, DCMPath.LastIndexOf("\\") + 1);
                    DCMPath = DCMPath + PNGName;
                    DCMPath = DCMPath.Replace(".png", ".dcm");

                    Console.WriteLine("PNGName：" + PNGName + "  DicomPath：" + DCMPath);
                    //-------------------------------------------------------------------
                    //讀XML－>List
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(XmlFileName_e.FullName);
                    List<SetInformation> infor = new List<SetInformation>();
                    XmlNodeList list = xdoc.SelectNodes("//annotation/object");
                    XmlNodeList listInner = xdoc.SelectNodes("//annotation/object/bndbox");

                    int count = 0;

                    foreach (XmlNode item in list)
                    {
                        SetInformation num1 = new SetInformation();
                        num1.JointName = item["name"].InnerText;

                        var dd = listInner[count];
                        var fu = dd["xmin"].InnerText;
                        num1.Xmin = Convert.ToInt32(dd["xmin"].InnerText);
                        num1.Ymin = Convert.ToInt32(dd["ymin"].InnerText);
                        num1.Xmax = Convert.ToInt32(dd["xmax"].InnerText);
                        num1.Ymax = Convert.ToInt32(dd["ymax"].InnerText);
                        infor.Add(num1);

                        count++;
                    }

                    //判斷類別與是否存在目錄
                    string f_type = "";
                    string NewFolder = "_xml";
                    string NFlocation = @"D:\LAB\mtss\mTss-file\";
                    String CopyDesDir = NFlocation + f_type + NewFolder;
                    if (XmlFileName_e.FullName.IndexOf("H", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (XmlFileName_e.FullName.IndexOf("EROSIONS", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            f_type = "EROSIONS";
                            CopyDesDir = NFlocation + f_type + NewFolder;
                            if (!(Directory.Exists(CopyDesDir)))
                            {
                                Directory.CreateDirectory(CopyDesDir);
                            }
                            foreach (SetInformation d in infor)
                            {

                                File.Copy(XmlFileName_e.FullName, Path.Combine(CopyDesDir, XmlFileName_e.Name));
                                //File.Move(DCMPath, NFlocation + f_type + NewFolder);
                            }
                        } 
                    }
                }
                catch (Exception ex_e)
                {
                    ErrorFile.Add(XmlFileName_e.FullName);
                }

            }
            foreach (string FileName in ErrorFile)
            {
                Console.WriteLine("出問題的檔案：" + FileName);
            }
            Console.WriteLine("End");


        }
    }
}

