using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.IO;
namespace MemoryScanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MemoryScanner memScan;
        MemoryReader memRead;
        Process p;
        List<Addresses.GetAddresses> list;
        Tests.PacketListnerServer packetlistner;
        Tests.PacketListnerClient packetlistnerOutgoing;



        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

         
            jumpBack:
            if(Process.GetProcessesByName("tibia").Length == 0)
            {
                MessageBox.Show("You must start a tibia client before proceed");
                goto jumpBack;
            }
            else
            {
                p = Process.GetProcessesByName("tibia")[0];
                memScan = new MemoryScanner(p);
                memRead = new MemoryReader(p);
                list = new List<Addresses.GetAddresses>();
                Util.GlobalVars.FullPath = p.Modules[0].FileName;
            }
            packetlistner = new Tests.PacketListnerServer(memRead);
            packetlistnerOutgoing = new Tests.PacketListnerClient(memRead);
            CheckForIllegalCrossThreadCalls = false;
            SetUp();
            }
            catch (Exception o)
            {
                MessageBox.Show(o.ToString());
                MessageBox.Show("Something went wrong, close all tibia clients and try again");
               
            }
           
        }
           
        private void SetUp()
        {
            Addresses.MyAddresses.AttackCount = new Addresses.AttackCount(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.GuiPointer = new Addresses.GuiPointer(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.CreatePacket  = new Addresses.CreatePacket(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.AddPacketByte  = new Addresses.AddPacketByte(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.Experience  = new Addresses.Experience(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.MapPointer  = new Addresses.MapPointer(memRead, memScan, Addresses.GetAddresses.AddressType.Map);
            Addresses.MyAddresses.MapArray  = new Addresses.MapArray(memRead, memScan, Addresses.GetAddresses.AddressType.Map);
            Addresses.MyAddresses.FullLight  = new Addresses.FullLight(memRead, memScan, Addresses.GetAddresses.AddressType.Map);
            Addresses.MyAddresses.StepTile  = new Addresses.StepTile(memRead, memScan, Addresses.GetAddresses.AddressType.Map);
            Addresses.MyAddresses.Mc  = new Addresses.Mc(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.WalkFunction  = new Addresses.WalkFunction(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.SendPacket  = new Addresses.SendPacket(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.Health  = new Addresses.Health(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.XorKey  = new Addresses.XorKey(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.Mana  = new Addresses.Mana(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.PlayerId  = new Addresses.PlayerId(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.PlayerX  = new Addresses.PlayerX(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.PlayerY  = new Addresses.PlayerY(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.PlayerZ  = new Addresses.PlayerZ(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.RedSqare  = new Addresses.RedSquare(memRead, memScan, Addresses.GetAddresses.AddressType.Player);
            Addresses.MyAddresses.GetnextPacket  = new Addresses.GetNextPacket(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.ParseFunction  = new Addresses.ParseFunction(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.Status  = new Addresses.Status(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.ReciveStream  = new Addresses.ReciveStream(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.PrintFPS  = new Addresses.PrintFps(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.PrintText  = new Addresses.PrintText(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.ShowFPS  = new Addresses.ShowFPS(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.NopFPS  = new Addresses.NopFps(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.BlistStart  = new Addresses.BlistStart(memRead, memScan, Addresses.GetAddresses.AddressType.BattleList);
            Addresses.MyAddresses.BlistStep  = new Addresses.BlistStep(memRead, memScan, Addresses.GetAddresses.AddressType.BattleList);
            Addresses.MyAddresses.MaxCreatures  = new Addresses.MaxCreatures(memRead, memScan, Addresses.GetAddresses.AddressType.BattleList);
            Addresses.MyAddresses.ContainerPointer  = new Addresses.ContainerPointer(memRead, memScan, Addresses.GetAddresses.AddressType.Container);
            Addresses.MyAddresses.RecivePointer  = new Addresses.RecivePointer(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.SendPointer  = new Addresses.SendPointer(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.StatusBarText  = new Addresses.StatusBarText(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.StatusBarTime  = new Addresses.StatusBarTime(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
            Addresses.MyAddresses.OutGoingBuffer  = new Addresses.OutgoingBuffer(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.OutGoingPacketLen  = new Addresses.OutGoingPacketLen(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
            Addresses.MyAddresses.XTEA  = new Addresses.Xtea(memRead, memScan, Addresses.GetAddresses.AddressType.Packet);
        }
        private void button1_Click(object sender, EventArgs e)
        {       
            treeView1.Nodes.Clear();
            list.Clear();
            StartSearch();
            groupBox1.Enabled = true;
        }
       
  
        private void StartSearch()
        {

            list.Add(Addresses.MyAddresses.AttackCount);
            list.Add(Addresses.MyAddresses.GuiPointer);
            list.Add(Addresses.MyAddresses.CreatePacket);
            list.Add(Addresses.MyAddresses.AddPacketByte);
            list.Add(Addresses.MyAddresses.Experience);
            list.Add(Addresses.MyAddresses.MapPointer);
            list.Add(Addresses.MyAddresses.MapArray);
            list.Add(Addresses.MyAddresses.FullLight);
            list.Add(Addresses.MyAddresses.StepTile);
            list.Add(Addresses.MyAddresses.Mc);
            list.Add(Addresses.MyAddresses.WalkFunction);
            list.Add(Addresses.MyAddresses.SendPacket);
            list.Add(Addresses.MyAddresses.Health);
            list.Add(Addresses.MyAddresses.XorKey);
            list.Add(Addresses.MyAddresses.Mana);
            list.Add(Addresses.MyAddresses.PlayerId);
            list.Add(Addresses.MyAddresses.PlayerX);
            list.Add(Addresses.MyAddresses.PlayerY);
            list.Add(Addresses.MyAddresses.PlayerZ);
            list.Add(Addresses.MyAddresses.RedSqare);
            list.Add(Addresses.MyAddresses.GetnextPacket);
            list.Add(Addresses.MyAddresses.ParseFunction);
            list.Add(Addresses.MyAddresses.Status);
            list.Add(Addresses.MyAddresses.ReciveStream);
            list.Add(Addresses.MyAddresses.PrintFPS);
            list.Add(Addresses.MyAddresses.PrintText);
            list.Add(Addresses.MyAddresses.ShowFPS);
            list.Add(Addresses.MyAddresses.NopFPS);
            list.Add(Addresses.MyAddresses.BlistStart);
            list.Add(Addresses.MyAddresses.BlistStep);
            list.Add(Addresses.MyAddresses.MaxCreatures);
            list.Add(Addresses.MyAddresses.ContainerPointer);
            list.Add(Addresses.MyAddresses.RecivePointer);
            list.Add(Addresses.MyAddresses.SendPointer);
            list.Add(Addresses.MyAddresses.StatusBarText);
            list.Add(Addresses.MyAddresses.StatusBarTime);
            list.Add(Addresses.MyAddresses.OutGoingBuffer);
            list.Add(Addresses.MyAddresses.OutGoingPacketLen);
            list.Add(Addresses.MyAddresses.XTEA);

            TreeNode ParentNode = new TreeNode();
            ParentNode.Text = p.MainModule.FileVersionInfo.FileVersion;

            TreeNode ClientParent = new TreeNode();           
            TreeNode BattlelistParent = new TreeNode();
            TreeNode PlayerParent = new TreeNode();
            TreeNode MapParent = new TreeNode();
            TreeNode ContainerParrent = new TreeNode();
            TreeNode PacketParrent = new TreeNode();
            ClientParent.Text = "ClientAddresses";
           
            BattlelistParent.Text = "BattlistAddress";
            PlayerParent.Text = "PlayerAddresses";
            MapParent.Text = "MapAddresses";
            ContainerParrent.Text = "ContainerAddresses";
            PacketParrent.Text = "Packet Addresses";
            for (int i = 0; i < list.Count; i++)
            {
             //   listBox1.Items.Add(list[i].GetString());
                Addresses.GetAddresses val = list[i];
                 TreeNode n;
                switch (val.AddressCategory)
                {
                   
                    case Addresses.GetAddresses.AddressType.Client:
                       n = new TreeNode();
                       n.Text = val.GetString();
                       ClientParent.Nodes.Add(n);
                    break;
                    case Addresses.GetAddresses.AddressType.BattleList:
                       n = new TreeNode();
                       n.Text = val.GetString();
                       BattlelistParent.Nodes.Add(n);
                    break;
                    case Addresses.GetAddresses.AddressType.Player:
                       n = new TreeNode();
                       n.Text = val.GetString();
                       PlayerParent.Nodes.Add(n);
                    break;
                    case Addresses.GetAddresses.AddressType.Map:
                       n = new TreeNode();
                       n.Text = val.GetString();
                       MapParent.Nodes.Add(n);
                    break;
                    case Addresses.GetAddresses.AddressType.Container:
                       n = new TreeNode();
                       n.Text = val.GetString();
                       ContainerParrent.Nodes.Add(n);
                    break;
                    case Addresses.GetAddresses.AddressType.Packet:
                        n = new TreeNode();
                       n.Text = val.GetString();
                       PacketParrent.Nodes.Add(n);
                    break;
                }
               
            }
            ParentNode.Nodes.Add(ClientParent);
            ParentNode.Nodes.Add(BattlelistParent);
            ParentNode.Nodes.Add(PlayerParent);
            ParentNode.Nodes.Add(MapParent);
            ParentNode.Nodes.Add(ContainerParrent);
            ParentNode.Nodes.Add(PacketParrent);
            treeView1.Nodes.Add(ParentNode);
            timer1.Start();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML files(.xml)|*.xml|all Files(*.*)|*.*";
            dlg.FilterIndex = 1;
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
            
                exportToXml(dlg.FileName);
            }          
        }
        private StreamWriter sr;
        public void exportToXml( string filename)
        {
            sr = new StreamWriter(filename, false, System.Text.Encoding.UTF8);
            //Write the header
            sr.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            //Write our root node
            sr.WriteLine("<" + "Addresses" + ">");
            foreach (TreeNode node in treeView1.Nodes)
            {
                sr.WriteLine("<" + node.Text + ">");
                saveNode(node.Nodes);
                sr.WriteLine("</" + node.Text + ">");
            }
            //Close the root node
            sr.WriteLine("</" + "Addresses" + ">");
            sr.Close();
        }

        private void saveNode(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                //If we have child nodes, we'll write 
                //a parent node, then iterrate through
                //the children
                if (node.Nodes.Count > 0)
                {
                    sr.WriteLine("<" + node.Text + ">");
                    saveNode(node.Nodes);
                    sr.WriteLine("</" + node.Text + ">");
                }
                else //No child nodes, so we just write the text
                    sr.WriteLine(node.Text);
            }
        
        }

        private void button3_Click(object sender, EventArgs e)
        {           
       
           if(Addresses.MyAddresses.Mc.CheckAddress())
           {
               MessageBox.Show("Mc Address seems to work");
           }
           else
           {
               MessageBox.Show("mc address error");
           }
          
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

 
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Util.GlobalVars.ShowWithBase = checkBox1.Checked;
        }

        private void addressCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode n = treeView1.SelectedNode;
            if (n != null)
            {
                string s = n.ToString();
                if (s.Contains("= 0x"))
                {
                    string[] split = s.Split('=');
                    Clipboard.SetText(split[1]);

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Tests.SendPacket sendPacket = new Tests.SendPacket(memRead);
            Util.Packet packet = new Util.Packet();
            packet.AddByte(0x96);
            packet.AddByte(0x1);
            packet.AddString("Hi");
            sendPacket.SendPacketToServer(packet.RawData);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Tests.PrintText PrintText = new Tests.PrintText(memRead);
            PrintText.CreateCodeCave(255, 1, 1, 200, 200, 2, "This is a test", "test");
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
            packetlistner.SetUpCodeCave();
            packetlistner.IncommingPacket += packetlistner_IncommingPacket;

        }
   
        void packetlistner_IncommingPacket(byte[] data)
        {
            string str = "";
            foreach (byte b in data)
            {
                str += b.ToString("X") + " ";
            }
         
            listBox1.Items.Add(str);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            packetlistner.CleanUp();
            packetlistnerOutgoing.CleanUp();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Tests.PlayerName playerName = new Tests.PlayerName(memRead);
            label1.Text = "Connection Status = " +memRead.ReadInt32(Addresses.MyAddresses.Status.Address).ToString();
            label2.Text = "PlayerX = " + memRead.ReadInt32(Addresses.MyAddresses.PlayerX.Address).ToString();
            label3.Text = "PlayerY = " + memRead.ReadInt32(Addresses.MyAddresses.PlayerY.Address).ToString();
            label4.Text = "PlayerZ = " + memRead.ReadInt32(Addresses.MyAddresses.PlayerZ.Address).ToString();
            label5.Text = "PlayerId = " + memRead.ReadInt32(Addresses.MyAddresses.PlayerId.Address).ToString();
            label6.Text = "PlayerName = " + playerName.GetPlayerName();
            int num = memRead.ReadInt32(Addresses.MyAddresses.GuiPointer.Address);
            num = memRead.ReadInt32(num + 0x30);
         
            Size clientSize = new Size(memRead.ReadInt32(num + 0x38), memRead.ReadInt32(num + 0x3C));
            label7.Text = "Client Size = " + clientSize.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            memRead.WriteString(Addresses.MyAddresses.StatusBarText.Address, "This is a test");
            memRead.WriteByte(Addresses.MyAddresses.StatusBarTime.Address, 40);
          
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Addresses.MyAddresses.WalkFunction.CheckAddress().ToString());
           
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            packetlistnerOutgoing.SetUpCodeCave();
            packetlistnerOutgoing.OutGoingPacket += lst_OutGoingPacket;
            
        }

        void lst_OutGoingPacket(byte[] data)
        {
            string str = "";
            foreach (byte b in data)
            {
                str += b.ToString("X") + " ";
            }

            listBox2.Items.Add(str);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            byte[] SearchBytes = new byte[] {0xC1,0xE8,0x07,0xA8,0x01,0x74,0x0F};
            List<int> values = memScan.ScanBytes(SearchBytes);
            if(values.Count > 0)
            {
                int adr = values[0] -26;
                Clipboard.SetText(adr.ToString("X"));
                MessageBox.Show(memRead.ReadInt32(adr).ToString("X"));

            }
        }
    }
}
