using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HTML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //выбор файла с текстом
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
            else
                MessageBox.Show(e.ToString());
        }

        private void button2_Click(object sender, EventArgs e) //выбор файла-словаря
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox2.Text = openFileDialog1.FileName;
            else
                MessageBox.Show(e.ToString());
        }

        private void button4_Click(object sender, EventArgs e) //установление максимального количества строк в html-файле
        {
            if (!(Convert.ToInt32(textBox3.Text) >= 10 & Convert.ToInt32(textBox3.Text) <= 100000))
                MessageBox.Show("Значение параметра должно быть в диапазоне от 10 до 100 000");
            else
                textBox3.ReadOnly = true;
        }

        private void button5_Click(object sender, EventArgs e) //изменение максимального количества строк
        {
            textBox3.ReadOnly = false;
        }

        private void button3_Click(object sender, EventArgs e) //создание html-файлов
        {
            if (textBox1.Text != "")
            {
                if (textBox2.Text != "")
                {
                    if (textBox3.Text != "")
                    {
                        string result = "";
                        int count = 0, numb = 0; //count - отсчет строк в файле, numb - имя файла html
                        string[] vocabulary = new string[File.ReadAllLines(textBox2.Text).Length];
                        //List<string> vocabulary = new List<string>();
                        using (StreamReader file_vocabulary = new StreamReader(textBox2.Text, Encoding.Default))
                        {
                            while (!file_vocabulary.EndOfStream)
                            {
                                vocabulary[count]=file_vocabulary.ReadLine();
                                count++;
                            }
                            count = 0;
                        }
                        System.IO.Directory.CreateDirectory(Application.StartupPath + '\\' + textBox3.Text);
                        string path = Application.StartupPath + '\\' + textBox3.Text + '\\' + numb.ToString() + ".html";
                        try
                        {
                            string[] HtmlResult = new string[File.ReadAllLines(textBox1.Text).Length];
                            using (StreamReader text = new StreamReader(textBox1.Text, Encoding.Default))
                            {
                                char[] Splitter = new char[] { ' ' , ',' , '!' , '?', '.' , ':' , ';', '(', ')', '+', '*' };
                                while (!text.EndOfStream)
                                {
                                        var word = "";
                                        var str = text.ReadLine();
                                        for (int i = 0; i < str.Length; i++)
                                        {
                                            if (!Splitter.Any(s=>s==str[i]))
                                                word += str[i];
                                            else
                                            {
                                                if (vocabulary.Any(w=>w==word))
                                                    result += "<b><i>" + word + @"</b></i>" + str[i];
                                                else
                                                    result += word + str[i];
                                                word = "";
                                            }
                                            if (i + 1 == str.Length)
                                            {
                                                result += word + "<br>";
                                                word = "";
                                            }
                                        }
                                        if (str == "")
                                            result = "<br>";
                                        HtmlResult[count]=result;
                                        result = "";
                                        count++;
                                }
                                count = 0;
                            }
                            var eatenString = "";
                                for (var i = 0; i < HtmlResult.Length; i++)
                                {
                                    StreamWriter writeFile = new StreamWriter(path, true, Encoding.Default);
                                    if (count != Convert.ToInt32(textBox3.Text))
                                    {
                                        count++;
                                        if (eatenString != "")
                                        {
                                            writeFile.WriteLine(eatenString);
                                            eatenString = "";
                                        }
                                        writeFile.WriteLine(HtmlResult[i]);
                                    }
                                    else
                                    {
                                        eatenString = HtmlResult[i];
                                        numb++;
                                        count = 0;
                                        path = Application.StartupPath + '\\' + textBox3.Text + '\\' + numb.ToString() + ".html";
                                    }
                                    writeFile.Close();
                                }
                        }
                        catch(Exception ee)
                        {
                            MessageBox.Show("В ходе выполнения программы произошла ошибка: " + ee.ToString());
                        }
                    }
                    else
                        MessageBox.Show("Введите максимальное количество строк в html файле!");
                }
                else
                    MessageBox.Show("Введите путь к файлу-словарю!");
            }
            else
                MessageBox.Show("Введите путь к файлу с текстом!");
        }

        private void button6_Click(object sender, EventArgs e) //выход из программы
        {
            Application.Exit();
        }
    }
}
