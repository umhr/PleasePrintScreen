using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace PleasePrintScreen
{
    public partial class Form1 : Form
    {
        private Timer _timer = new Timer();
        private String _logFilePath;
        private String _saveDirectoryPath;
        public Form1()
        {
            InitializeComponent();

            setPath();
        }
        private void setPath()
        {
            string user = Environment.UserName;
            _logFilePath = "C:\\Users\\" + user + "\\AppData\\Roaming\\Macromedia\\Flash Player\\Logs\\flashlog.txt";
            label3.Text = _logFilePath;

            _saveDirectoryPath = "C:\\Users\\" + user + "\\Desktop";
            label4.Text = _saveDirectoryPath;

        }
        private void setTimer()
        {
            _timer.Interval = (int)numericUpDown1.Value;
            _timer.Tick += tick;
            if (_timer.Enabled)
            {
                _timer.Stop();
                button2.Text = "Start";
            }
            else
            {
                _timer.Start();
                button2.Text = "Stop";
            }
        }

        private void tick(object sender, EventArgs e)
        {
            getFlashlog();
        }

        private String _log = "";
        private void getFlashlog()
        {
            // エラー処理（例外処理）の基本: .NET Tips: C#, VB.NET
            //http://dobon.net/vb/dotnet/beginner/exceptionhandling.html
            try
            {
                //ファイルを開く
                StreamReader sr = new StreamReader(_logFilePath);
                readFile(sr.ReadToEnd());
                sr.Close();
            }
            catch (System.IO.IOException ex)
            {
                //IOExceptionをキャッチした時
                textBox1.AppendText("ファイルがロックされていて読めませんでした。\n");
                System.Console.WriteLine(ex.Message);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                //UnauthorizedAccessExceptionをキャッチした時
                textBox1.AppendText("必要なアクセス許可がありません。\n");
                System.Console.WriteLine(ex.Message);
            }

        }
        private void readFile(String text)
        {
            String distanceText;

            if (text.Length >= _log.Length)
            {
                if (textBox1.TextLength > 4000)
                {
                    textBox1.Text = textBox1.Text.Substring(2000);
                }

                distanceText = text.Substring(_log.Length);
                textBox1.AppendText(distanceText);
            }
            else
            {
                distanceText = text;
                textBox1.Text = distanceText;
            }
            
            var keyword = textBox2.Text;

            if (distanceText.IndexOf(keyword) > -1)
            {
                capture();
            }

            _log = text;
        }

        private void capture()
        {
            // 画面をキャプチャする: .NET Tips: C#, VB.NET
            //http://dobon.net/vb/dotnet/graphics/screencapture.html

            //Bitmapの作成
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);
            //Graphicsの作成
            Graphics g = Graphics.FromImage(bmp);
            //画面全体をコピーする
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), bmp.Size);
            //解放
            g.Dispose();

            DateTime dtNow = DateTime.Now;
            String now = dtNow.ToString("yyyyMMdd_HHmmss") + (1000 + dtNow.Millisecond).ToString().Substring(1);
            String fileName = _saveDirectoryPath + "\\printscreen" + now + ".png";

            try
            {
                //PNG形式で保存する
                bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (System.IO.IOException ex)
            {
                //IOExceptionをキャッチした時
                textBox1.AppendText("ファイルがロックされている可能性があります。\n");
                System.Console.WriteLine(ex.Message);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                //UnauthorizedAccessExceptionをキャッチした時
                textBox1.AppendText("必要なアクセス許可がありません。\n");
                System.Console.WriteLine(ex.Message);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                textBox1.AppendText("ディレクトリの必要なアクセス許可がありません。\n");
                System.Console.WriteLine(ex.Message);
            }

            //後片付け
            bmp.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setTimer();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ＠IT：.NET TIPS マシン名／ユーザー名を取得するには？ - C#
            //http://www.atmarkit.co.jp/fdotnet/dotnettips/055machinename/machinename.html
            // 「ファイルを開く」ダイアログボックスを表示する: .NET Tips: C#, VB.NET
            //http://dobon.net/vb/dotnet/form/openfiledialog.html

            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            ofd.FileName = "flashlog.txt";

            string user = Environment.UserName;

            string InitialDirectory = @"C:\\Users\\" + user + "\\AppData\\Roaming\\Macromedia\\Flash Player\\Logs\\";

            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = InitialDirectory;//@"C:\";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "flashlogファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに
            //「すべてのファイル」が選択されているようにする
            ofd.FilterIndex = 1;
            //タイトルを設定する
            ofd.Title = "flashlog.txt";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _logFilePath = ofd.FileName;
                label3.Text = _logFilePath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //上部に表示する説明テキストを指定する
            fbd.Description = "フォルダを指定してください。";
            //ルートフォルダを指定する
            //デフォルトでDesktop
            fbd.RootFolder = Environment.SpecialFolder.Desktop;

            string user = Environment.UserName;

            string SelectedPath = @"C:\\Users\\" + user + "\\Desktop";

            //最初に選択するフォルダを指定する
            //RootFolder以下にあるフォルダである必要がある
            fbd.SelectedPath = SelectedPath;// @"C:\Windows";
            //ユーザーが新しいフォルダを作成できるようにする
            //デフォルトでTrue
            fbd.ShowNewFolderButton = true;

            //ダイアログを表示する
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                _saveDirectoryPath = fbd.SelectedPath;
                label4.Text = _saveDirectoryPath;
            }
        }
    }
}
