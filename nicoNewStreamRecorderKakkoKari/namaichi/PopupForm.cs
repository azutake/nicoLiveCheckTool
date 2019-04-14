﻿/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2019/01/14
 * Time: 3:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using namaichi.info;
using namaichi.alart;

namespace namaichi
{
	/// <summary>
	/// Description of BalloonForm.
	/// </summary>
	public partial class PopupForm : PopupFormBase
	{
//		private config.config config;
//		private RssItem ri; 
		public PopupForm(RssItem item, config.config config, 
				PopupDisplay pd, int showIndex, AlartInfo ai,
				bool isTest = false, string poploc = null, int poptime = 0,
				bool isClickClose = true,  
				bool isTopMost = true, bool isColor = true, double opacity = 0.9)
		{
			this.config = config;
			this.ri = item;
			this.pd = pd;
			this.showIndex = showIndex;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//var dt = DateTime.Now;
			var dtStr = "";
			if (item.pubDate != null && item.pubDate != "") {
				var dt = DateTime.Parse(item.pubDate);
				dtStr = dt.ToString("yyyy/MM/dd(ddd) HH:mm") + "開始";
			}
			util.debugWriteLine(dtStr);
			
			//var t = "【弾幕歓迎】BOOWY関係リク枠！放送のしかた覚えてるかな？覚えてなかったらごめんlt;(_ _)gt;";
			var t = item.title;
			if (isOkStrWidth(t + " - ")) t += " - ";
			if (isOkStrWidth(t + dtStr)) t += dtStr;
			titleLabel.Text = t;
			
			hostNameLabel.Text = item.hostName;
			communityNameLabel.Text = item.comName;
			descryptionLabel.Text = item.description;
			//var _Text = item.hostName + "/" + item.comName;
			if (ai != null && ai.keyword != null && ai.keyword != "") Text = ai.keyword + "-" + Text;
			if (ai != null && bool.Parse(config.get("IsColorPopup"))) {
				BackColor = ai.backColor;
				ForeColor = ai.textColor;
			}
			//Text = _Text;
			if (isTest && isColor) {
				BackColor = Color.FromArgb(255,224,255);
				ForeColor = Color.Black;
			} else if (ai != null && bool.Parse(config.get("IsColorPopup"))) {
				BackColor = ai.backColor;
				ForeColor = ai.textColor;
			}
			
			var url = "https://live2.nicovideo.jp/watch/" + item.lvId;
			titleLabel.Links.Add(0, titleLabel.Text.Length, url);
			if (isTest) TopMost = isTopMost;
			else TopMost = bool.Parse(config.get("IsTopMostPopup"));
			
			Opacity = isTest ? (opacity / 100) : double.Parse(config.get("popupOpacity")) / 100;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			this.isTestClosePopup = isClickClose;
			this.testPopTime = poptime;
			this.isTest = isTest;
			
			ContextMenuStrip = contextMenuStrip1;
			setAppliMenuVisible();
		}
		private bool isOkStrWidth(string s) {
			var w = TextRenderer.MeasureText(s, titleLabel.Font).Width;
			return (w < titleLabel.Width * 2 - 2); 
		}
		public void setSamune(string url) {
			//if (string.IsNullOrEmpty(url)) return;
			if (isTest) {
				thumbnailPictureBox.Image = new Bitmap(Image.FromFile(ri.thumbnailUrl), thumbnailPictureBox.Size);
				return;
			}
			Image img;
			var dt0 = DateTime.Now;
			if (!ThumbnailManager.isExist(ri.comId, out img)) {
				util.debugWriteLine("is not exist thumbnail time " + (DateTime.Now - dt0) + " " + ri.comName);
				
				img = ThumbnailManager.getImageId(ri.comId);
				if (img != null) ThumbnailManager.saveImage(img, ri.comId);
				if (img == null && !string.IsNullOrEmpty(url)) 
					img = ThumbnailManager.getThumbnailRssUrl(url);
			} else {
				util.debugWriteLine("is exist thumbnail time " + (DateTime.Now - dt0) + " " + ri.comName);
			}
			thumbnailPictureBox.Image = img;
			
			util.debugWriteLine("is set thumbnail time " + (DateTime.Now - dt0) + " " + ri.comName);
			/*
       		if (!util.isShowWindow) return;
       		if (IsDisposed) return;
       		WebClient cl = new WebClient();
       		cl.Proxy = null;
			
       		System.Drawing.Icon icon =  null;
			try {
       			byte[] pic = cl.DownloadData(url);
				
			
				var  st = new System.IO.MemoryStream(pic);
				icon = Icon.FromHandle(new System.Drawing.Bitmap(st).GetHicon());
				st.Close();
				
				
			} catch (Exception e) {
				util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
				util.debugWriteLine("set samune error " + url);
				return;
			}
			
			try {
	        	Invoke((MethodInvoker)delegate() {
       			       	try {
						    thumbnailPictureBox.Image = icon.ToBitmap();
							
       			       	} catch (Exception e) {
       			       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
       			       	}
       			});
			} catch (Exception e) {
       			util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
       		}
					
       			
//       			Icon = new System.Drawing.Icon(url);
			*/
		}
		
		void TitleLabelLinkClicked(object _sender, LinkLabelLinkClickedEventArgs e)
		{
			LinkLabel sender = (LinkLabel)_sender;
			if (e.Button == MouseButtons.Left) {
				if (sender.Links.Count > 0 && sender.Links[0].Length != 0) {
					string url = (string)sender.Links[0].LinkData;
					util.openUrlBrowser(url, config);
				}
				if ((isTest && isTestClosePopup) ||
			    		(!isTest && config.get("Isclosepopup") == "true"))
					Close();
			} else {
//				if (sender.Links.Count > 0 && sender.Links[0].Length != 0) {
//					labelUrl = (string)sender.Links[0].LinkData;
//					mainWindowRightClickMenu.Show(Cursor.Position);
//				}
			}
		}
		void timeoutCloseProcess() {
			var t = (isTest) ? testPopTime : int.Parse(config.get("poptime"));
			Thread.Sleep(t * 1000);
			for (var o = Opacity; o > -0.00; o -= 0.05) {
				setOpacity(o);
				Thread.Sleep(100);
			}
			_close();
			
		}
		void setOpacity(double o) {
			try {
				if (IsDisposed || Disposing) return;
	        	Invoke((MethodInvoker)delegate() {
       			       	try {
						    Opacity = o;
       			       	} catch (Exception e) {
       			       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
       			       	}
       			});
			} catch (Exception e) {
       			util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
       		}
		}
		void _close() {
			try {
				if (IsDisposed || Disposing) return;
	        	Invoke((MethodInvoker)delegate() {
       			       	try {
				       		Close();
       			       	} catch (Exception e) {
       			       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
       			       	}
       			});
			} catch (Exception e) {
       			util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
       		}
		}
		
		void PopupFormLoad(object sender, EventArgs e)
		{
			setSamune(ri.thumbnailUrl);
			//Task.Run(() => setSamune(ri.thumbnailUrl));
			Task.Run(() => timeoutCloseProcess());
		}
		
		void PopupFormFormClosed(object sender, FormClosedEventArgs e)
		{
			pd.posList.Remove(showIndex);
		}
		
	}
}
