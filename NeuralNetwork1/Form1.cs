using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.WindowsForms;

namespace NeuralNetwork1
{

	public delegate void FormUpdater(double progress, double error, TimeSpan time);

    public delegate void UpdateTLGMessages(string msg);

    public partial class Form1 : Form
    {
        /// <summary>
        /// Чат-бот AIML
        /// </summary>
        AIMLBotik botik = new AIMLBotik();

        TLGBotik tlgBot;

        /// <summary>
        /// Генератор изображений (образов)
        /// </summary>
        //GenerateImage generator = new GenerateImage();
        
        /// <summary>
        /// Обёртка для ActivationNetwork из Accord.Net
        /// </summary>
        AccordNet AccordNet = null;

        MyNeuralNetwork customnet = null;
        SamplesSet ss;

        /// <summary>
        /// Абстрактный базовый класс, псевдоним либо для CustomNet, либо для AccordNet
        /// </summary>
        BaseNetwork net = null;

        public Form1()
        {
            InitializeComponent();
            tlgBot = new TLGBotik(net, new UpdateTLGMessages(UpdateTLGInfo));
            //netTypeBox.SelectedIndex = 1;
            //generator.figure_count = (int)classCounter.Value;
            button3_Click(this, null);
            //pictureBox1.Image = Properties.Resources.Title;
            pictureBox1.Image = Properties.Resources.kek;
        }

		public void UpdateLearningInfo(double progress, double error, TimeSpan elapsedTime)
		{
			if (progressBar1.InvokeRequired)
			{
				progressBar1.Invoke(new FormUpdater(UpdateLearningInfo),new Object[] {progress, error, elapsedTime});
				return;
			}
            StatusLabel.Text = "Accuracy: " + error.ToString();
            int prgs = (int)Math.Round(progress*100);
			prgs = Math.Min(100, Math.Max(0,prgs));
            elapsedTimeLabel.Text = "Затраченное время : " + elapsedTime.Duration().ToString(@"hh\:mm\:ss\:ff");
            progressBar1.Value = prgs;
		}

        public void UpdateTLGInfo(string message)
        {
            if (TLGUsersMessages.InvokeRequired)
            {
                TLGUsersMessages.Invoke(new UpdateTLGMessages(UpdateTLGInfo), new Object[] { message });
                return;
            }
            TLGUsersMessages.Text += message + Environment.NewLine;
        }

        private void set_result(Sample figure)
        {
            label1.Text = figure.ToString();

            if (figure.Correct())
                label1.ForeColor = Color.Green;
            else
                label1.ForeColor = Color.Red;

            label1.Text = "Распознано : " + figure.recognizedClass.ToString();

            //label8.Text = String.Join("\n", net.getOutput().Select(d => d.ToString()));
            //pictureBox1.Image = generator.genBitmap();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //Sample fig = generator.GenerateFigure();

            //net.Predict(fig);

            //set_result(fig);

            /*var rnd = new Random();
            var fname = "pic" + (rnd.Next() % 100).ToString() + ".jpg";
            pictureBox1.Image.Save(fname);*/

        }

        private async Task<double> train_networkAsync(int training_size, int epoches, double acceptable_error, bool parallel = true)
        {
            //  Выключаем всё ненужное
            label1.Text = "Выполняется обучение...";
            label1.ForeColor = Color.Red;
            
            pictureBox1.Enabled = false;
      

            //  Создаём новую обучающую выборку
            SamplesSet samples = new SamplesSet();

            //for (int i = 0; i < training_size; i++)
            //    samples.AddSample(generator.GenerateFigure());

            //  Обучение запускаем асинхронно, чтобы не блокировать форму
            double f = await Task.Run(() => net.TrainOnDataSet(ss, epoches, acceptable_error, parallel));

            label1.Text = "Щелкните на картинку для теста нового образа";
            label1.ForeColor = Color.Green;
           
            pictureBox1.Enabled = true;
         
            StatusLabel.Text = "Accuracy: " + f.ToString();
            StatusLabel.ForeColor = Color.Green;
            return f;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            train_networkAsync( 1, 30, 0.03, true);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            //  Проверяем корректность задания структуры сети
            //int[] structure = netStructureBox.Text.Split(';').Select((c) => int.Parse(c)).ToArray();
            //if (structure.Length < 2 || structure[0] != 400 || structure[structure.Length - 1] != generator.figure_count)
            //{
            //    MessageBox.Show("А давайте вы структуру сети нормально запишите, ОК?", "Ошибка", MessageBoxButtons.OK);
            //    return;
            //};

            //AccordNet = new AccordNet(structure);
            //AccordNet.updateDelegate = UpdateLearningInfo;

            //net = AccordNet;
            //customnet = new MyNeuralNetwork(new int[] { 400, 700, 50, 10 });
            customnet = new MyNeuralNetwork(new int[] { 400, 700, 100, 10 }); //пример, lr-0.25, alpha - 0.15
            customnet.updateDelegate = UpdateLearningInfo;

            AccordNet = new AccordNet(new int[] { 400, 700, 200, 50, 10 });
            AccordNet.updateDelegate = UpdateLearningInfo;

            net = customnet;

            tlgBot.SetNet(net);

        }

        private void classCounter_ValueChanged(object sender, EventArgs e)
        {
            //generator.figure_count = (int)classCounter.Value;
            //var vals = netStructureBox.Text.Split(';');
            //int outputNeurons;
            //if (int.TryParse(vals.Last(),out outputNeurons))
            //{
            //    vals[vals.Length - 1] = classCounter.Value.ToString();
            //    netStructureBox.Text = vals.Aggregate((partialPhrase, word) => $"{partialPhrase};{word}");
            //}
        }

        private void btnTrainOne_Click(object sender, EventArgs e)
        {
            //if (net == null) return;
            //Sample fig = generator.GenerateFigure();
            //pictureBox1.Image = generator.genBitmap();
            //pictureBox1.Invalidate();
            //net.Train(fig, false);
            //set_result(fig);
        }

        private void netTrainButton_MouseEnter(object sender, EventArgs e)
        {
            infoStatusLabel.Text = "Обучить нейросеть с указанными параметрами";
        }

        private void testNetButton_MouseEnter(object sender, EventArgs e)
        {
            infoStatusLabel.Text = "Тестировать нейросеть на тестовой выборке такого же размера";
        }

        private void netTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            net = AccordNet;
        }

        private void recreateNetButton_MouseEnter(object sender, EventArgs e)
        {
            infoStatusLabel.Text = "Заново пересоздаёт сеть с указанными параметрами";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var phrase = AIMLInput.Text;
            if (phrase.Length > 0)
                AIMLOutput.Text += botik.Talk(phrase) + Environment.NewLine;
        }

        private void TLGBotOnButton_Click(object sender, EventArgs e)
        {
            tlgBot.Act();
            TLGBotOnButton.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SamplerConverter sc = new SamplerConverter();
            ss = sc.ConvertSet(@"..\..\NewImages\");
        }
    }

  }
