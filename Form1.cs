using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe
{
    public partial class Form1 : Form
    {
        const double TEA_PRICE = 1.00;
        const double COFFEE_PRICE = 2.00;
        const double JUICE_PRICE = 3.00;

        double total;
        double discount;
        int luckyNumber;
        string luckyMessage;
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lstOrderList.Items.Count == 0)
            {
                MessageBox.Show("No order to save.");
                return;
            }

            try
            {
                StreamWriter outputFile = File.AppendText("orders.txt");

                foreach (object item in lstOrderList.Items)
                {
                    outputFile.WriteLine(item.ToString());
                }

                outputFile.WriteLine(lblTotalPrice.Text);
                outputFile.WriteLine(lblLuckyMessage.Text);

                outputFile.Close();

                MessageBox.Show("Order saved.");
            }
            catch
            {
                MessageBox.Show("Error saving the file.");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("orders.txt") == false)
                {
                    MessageBox.Show("No file found.");
                    return;
                }

               
                lstOrderList.Items.Clear();

                StreamReader inputFile = File.OpenText("orders.txt");

                while (inputFile.EndOfStream == false)
                {
                    string line = inputFile.ReadLine();

                    lstOrderList.Items.Add(line);
                }

                inputFile.Close();
            }
            catch
            {
                MessageBox.Show("Error loading the file.");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomerName.Clear();

            txtQuantity.Clear();

            lstOrderList.Items.Clear();

            lblTotalPrice.Text = "";

            lblLuckyMessage.Text = "";

            chkStudentMember.Checked = false;

            total = 0;

            luckyMessage = "";
        }



        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtCustomerName.Text.Trim();

                if (name == "")
                {
                    MessageBox.Show("Enter customer name.");
                    return;
                }

                foreach (char letter in name)
                {
                    if (!char.IsLetter(letter) && letter != ' ')
                    {
                        MessageBox.Show("Customer name must contain letters only.");
                        txtCustomerName.Clear();
                        txtCustomerName.Focus();
                        return;
                    }
                }

                int quantity = Convert.ToInt32(txtQuantity.Text);

                if (quantity < 1)
                {
                    MessageBox.Show("Enter quantity greater than zero.");
                    return;
                }

                double drinkPrice = 0;
                string drinkName = "";

                if (rdoTea.Checked == true)
                {
                    drinkPrice = TEA_PRICE;
                    drinkName = "Tea";
                }
                else if (rdoCoffee.Checked == true)
                {
                    drinkPrice = COFFEE_PRICE;
                    drinkName = "Coffee";
                }
                else if (rdoJuice.Checked == true)
                {
                    drinkPrice = JUICE_PRICE;
                    drinkName = "Juice";
                }
                else
                {
                    MessageBox.Show("Choose a drink.");
                    return;
                }

                lstOrderList.Items.Clear();

              
                Random lucky = new Random();

                int winningNumber = 0;

                for (int count = 1; count <= quantity; count++)
                {
                    int number = lucky.Next(1, 51);

                    lstOrderList.Items.Add(count + ") " + name + " - " +
                        drinkName + " - Lucky: " + number + " - $" +
                        drinkPrice.ToString("F2"));

                    if (number == 7 || number == 17)
                    {
                        winningNumber = number;
                    }
                }

               
                double beforeDiscount = drinkPrice * quantity;

                double discountAmount = 0;

                if (chkStudentMember.Checked == true)
                {
                    discountAmount = beforeDiscount * 0.10;
                }

                total = beforeDiscount - discountAmount;

                lstOrderList.Items.Add("------------------------");

                if (chkStudentMember.Checked == true)
                {
                    lstOrderList.Items.Add("Member Discount: 10%");
                }
                else
                {
                    lstOrderList.Items.Add("Member Discount: 0%");
                }

                lstOrderList.Items.Add("Total Before Discount: $" +
                    beforeDiscount.ToString("F2"));

                lstOrderList.Items.Add("Discount Amount: $ " +
                    discountAmount.ToString("F2"));

                lstOrderList.Items.Add("Total After Discount: $" +
                    total.ToString("F2"));

                if (winningNumber != 0)
                {
                    luckyMessage = "You got a Free Cookie! (" +
                        winningNumber + ")";
                }
                else
                {
                    luckyMessage = "No prize this time.";
                }

                lstOrderList.Items.Add("Lucky Message: " + luckyMessage);

                lblTotalPrice.Text = "Total Price: $" + total.ToString("F2");

                lblLuckyMessage.Text = luckyMessage;
            }
            catch
            {
                MessageBox.Show("Please enter a valid quantity.");
            }
        }

    }
    }
