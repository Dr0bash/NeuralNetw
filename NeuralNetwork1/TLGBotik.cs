using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using AForge.WindowsForms;

namespace NeuralNetwork1
{
    class TLGBotik
    {
        public Telegram.Bot.TelegramBotClient botik = null;

        private UpdateTLGMessages formUpdater;

        private BaseNetwork perseptron = null;

        private AIMLBotik mybot = null;

        public TLGBotik(BaseNetwork net, UpdateTLGMessages updater)
        { 
            var botKey = System.IO.File.ReadAllText("botkey.txt");
            mybot = new AIMLBotik();
            botik = new Telegram.Bot.TelegramBotClient(botKey);
            botik.OnMessage += Botik_OnMessageAsync;
            //botik.OnMessage += MyBotik_OnMessageAsync;
            formUpdater = updater;
            perseptron = net;
        }

        public void SetNet(BaseNetwork net)
        {
            perseptron = net;
            formUpdater("Net updated!");
        }

        private void Botik_OnMessageAsync(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            //  Тут очень простое дело - банально отправляем назад сообщения
            var message = e.Message;
            formUpdater("Тип сообщения : " + message.Type.ToString());

            //  Получение файла (картинки)
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
            {
                formUpdater("Picture loadining started");
                var photoId = message.Photo.Last().FileId;
                File fl = botik.GetFileAsync(photoId).Result;

                var img = System.Drawing.Image.FromStream(botik.DownloadFileAsync(fl.FilePath).Result);

                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(img);
                System.Drawing.Bitmap uProcessed;
                //  Масштабируем aforge
                try
                {
                    uProcessed = MagicEye.ProcessDataSetImage(bm);
                }
                catch
                {
                    botik.SendTextMessageAsync(message.Chat.Id, "Глаз замылился");
                    return;
                }
                uProcessed.Save(@"..\..\1.jpg");

                Sample sample = SamplerConverter.Convert(uProcessed);
                
                var x = perseptron.Predict(sample);
                switch (x)
                {
                    case FigureType.zero: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на нолик"); break;
                    case FigureType.one: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на единичку"); break;
                    case FigureType.two: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на двойку"); break;
                    case FigureType.three: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на тройку"); break;
                    case FigureType.four: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на четверку"); break;
                    case FigureType.five: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на пятерку"); break;
                    case FigureType.six: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на шестерку"); break;
                    case FigureType.seven: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на семерку"); break;
                    case FigureType.eight: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на восьмерку"); break;
                    case FigureType.nine: botik.SendTextMessageAsync(message.Chat.Id, "Похоже на девятку"); break;
                    default: botik.SendTextMessageAsync(message.Chat.Id, "Глаз замылился"); break;
                }
                string textresults = "";
                var outputs = perseptron.getOutput();
                textresults = "Распознанная цифра: " + x.ToString() + "\r\n";
                textresults += " 0: " + outputs[0].ToString() + "\r\n";
                textresults += " 1: " + outputs[1].ToString() + "\r\n";
                textresults += " 2: " + outputs[2].ToString() + "\r\n";
                textresults += " 3: " + outputs[3].ToString() + "\r\n";
                textresults += " 4: " + outputs[4].ToString() + "\r\n";
                textresults += " 5: " + outputs[5].ToString() + "\r\n";
                textresults += " 6: " + outputs[6].ToString() + "\r\n";
                textresults += " 7: " + outputs[7].ToString() + "\r\n";
                textresults += " 8: " + outputs[8].ToString() + "\r\n";
                textresults += " 9: " + outputs[9].ToString() + "\r\n";
                formUpdater(textresults);
                //botik.SendPhotoAsync(message.Chat.Id, @"..\..\1.jpg");
                formUpdater("Picture recognized!");
                return;
            }
            else
            {
                if (message == null || message.Type != Telegram.Bot.Types.Enums.MessageType.Text) return;
                if (message.Text == "Authors")
                {
                    string authors = "Гаянэ Аршакян, Луспарон Тызыхян, Дамир Казеев, Роман Хыдыров, Владимир Садовский, Анастасия Аскерова, Константин Бервинов, и Борис Трикоз (но он уже спать ушел) и молчаливый Даниил Ярошенко";
                    botik.SendTextMessageAsync(message.Chat.Id, "Авторы проекта : " + authors);
                }
                botik.SendTextMessageAsync(message.Chat.Id, mybot.Talk(message.Text));
                formUpdater(message.Text);
            }
            return;
        }

        private void MyBotik_OnMessageAsync(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            //  Тут очень простое дело - банально отправляем назад сообщения
            var message = e.Message;

            botik.SendTextMessageAsync(message.Chat.Id, mybot.Talk(message.Text));

            if (message == null || message.Type != Telegram.Bot.Types.Enums.MessageType.Text) return;
            if (message.Text == "Authors")
            {
                string authors = "Гаянэ Аршакян, Луспарон Тызыхян, Дамир Казеев, Роман Хыдыров, Владимир Садовский, Анастасия Аскерова, Константин Бервинов, и Борис Трикоз (но он уже спать ушел) и молчаливый Даниил Ярошенко";
                botik.SendTextMessageAsync(message.Chat.Id, "Авторы проекта : " + authors);
            }
            formUpdater(message.Text);
            return;
        }

        public bool Act()
        {
            try
            {
                botik.StartReceiving();
            }
            catch(Exception e) { 
                return false;
            }
            return true;
        }

        public void Stop()
        {
            botik.StopReceiving();
        }

    }
}
