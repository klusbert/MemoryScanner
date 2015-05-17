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
        Tests.PacketListner packetlistner;
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
             packetlistner = new Tests.PacketListner(memRead);
            CheckForIllegalCrossThreadCalls = false;
            }
            catch (Exception o)
            {
                MessageBox.Show(o.ToString());
                MessageBox.Show("Something went wrong, close all tibia clients and try again");
               
            }
           
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
           
            list.Add(new Addresses.AttackCount(memRead,memScan,Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.CreatePacket(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.AddPacketByte(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.Experience(memRead, memScan, Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.MapPointer(memRead, memScan, Addresses.GetAddresses.AddressType.Map));
            list.Add(new Addresses.MapArray(memRead, memScan, Addresses.GetAddresses.AddressType.Map));
            list.Add(new Addresses.FullLight(memRead, memScan, Addresses.GetAddresses.AddressType.Map));
            list.Add(new Addresses.StepTile(memRead, memScan, Addresses.GetAddresses.AddressType.Map));
            list.Add(new Addresses.Mc(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.WalkFunction(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.SendPacket(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.Health(memRead, memScan, Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.XorKey(memRead, memScan, Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.Mana(memRead, memScan, Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.PlayerId(memRead, memScan, Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.PlayerX(memRead, memScan, Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.PlayerY(memRead, memScan, Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.PlayerZ(memRead, memScan, Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.RedSquare(memRead, memScan, Addresses.GetAddresses.AddressType.Player));
            list.Add(new Addresses.GetNextPacket(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.ParseFunction(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.Status(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.ReciveStream(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.PrintFps(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.PrintText(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.ShowFPS(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.NopFps(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.BlistStart(memRead, memScan, Addresses.GetAddresses.AddressType.BattleList));
            list.Add(new Addresses.BlistStep(memRead, memScan, Addresses.GetAddresses.AddressType.BattleList));
            list.Add(new Addresses.MaxCreatures(memRead, memScan, Addresses.GetAddresses.AddressType.BattleList));
            list.Add(new Addresses.ContainerPointer(memRead, memScan, Addresses.GetAddresses.AddressType.Container));
            list.Add(new Addresses.RecivePointer(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.SendPointer(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.StatusBarText(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.StatusBarTime(memRead, memScan, Addresses.GetAddresses.AddressType.Client));   
            list.Add(new Addresses.OutgoingBuffer(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.OutGoingPacketLen(memRead, memScan, Addresses.GetAddresses.AddressType.Client));
            list.Add(new Addresses.Xtea(memRead, memScan, Addresses.GetAddresses.AddressType.Client)); 
            TreeNode ClientParent = new TreeNode();           
            TreeNode BattlelistParent = new TreeNode();
            TreeNode PlayerParent = new TreeNode();
            TreeNode MapParent = new TreeNode();
            TreeNode ContainerParrent = new TreeNode();
            ClientParent.Text = "ClientAddresses";
           
            BattlelistParent.Text = "BattlistAddress";
            PlayerParent.Text = "PlayerAddresses";
            MapParent.Text = "MapAddresses";
            ContainerParrent.Text = "ContainerAddresses";
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
                }
               
            }
            treeView1.Nodes.Add(ClientParent);
            treeView1.Nodes.Add(BattlelistParent);
            treeView1.Nodes.Add(PlayerParent);
            treeView1.Nodes.Add(MapParent);
            treeView1.Nodes.Add(ContainerParrent);
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
            Addresses.GetAddresses mctest = new Addresses.Mc(memRead, memScan, Addresses.GetAddresses.AddressType.Client);
           if(mctest.CheckAddress())
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
        }

     
    }
}
