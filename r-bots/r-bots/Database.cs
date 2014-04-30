using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace r_bots
{
    class Database
    {
        private MySqlConnection con;
        private MySqlCommand command;
        private DataSet database;
        private RichTextBox message;

        public Database(ref RichTextBox msg)
        {
            this.message = msg;
            string connectionString = "Server=localhost;Database=r_bots_db;Uid=root;Pwd=;";
            this.message.Text += "Initialisation de connexion à la base de données\n";
            this.con = new MySqlConnection(connectionString);
            database = new DataSet();
            try
            {
                con.Open();
                this.message.Text += "Ouverture de la base de données\n";
                command = new MySqlCommand();

                this.message.Text += "Insertion des nouvelles valeurs dans la base de données\n";
                try
                {
                    this.InsertNewValue("personne");
                }
                catch (Exception ex) { };
                this.message.Text += "Mise à jour de la base de données\n";
                this.Fill("personne");

                this.message.Text += "Toutes les données sont à jour\n";
                database.WriteXml("database.xml");
                database.WriteXmlSchema("databasestructure.xml");
            }
            catch (Exception ex)
            {
                this.message.Text += "Erreur:\n***********\n"+ex.Message+"\n";
            }
        }

        private void Fill(string table)
        {
            this.message.Text += "Retrait des données dans la table " + table + "\n";
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM " + table, con);
            adapter.Fill(database, table);
            this.message.Text += "Retrait achevés\n";
        }

        private void InsertNewValue(string table)
        {
            this.message.Text += "Insertion des données dans la table " + table + "\n";
            DataSet news= new DataSet();
            news.ReadXml("databaseinsert.xml");
            news.ReadXmlSchema("databasestructure.xml");
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM " + table, con);
                MySqlCommandBuilder cmd = new MySqlCommandBuilder(adapter);
                adapter.InsertCommand = new MySqlCommand("SELECT * FROM " + table);
                adapter.Update(news, table);
            }
            catch
            {
                this.message.Text += "Aucune valeur à inserer dans la table "+table+"\n";
            }
            this.message.Text += "Insertion achevées\n";
        }

        public void Insert(DataRow from, string table)
        {
            DataSet news = new DataSet();
            try
            {
                news.ReadXml("databaseinsert.xml");
            }
            catch (IOException ex) { }
            news.ReadXmlSchema("databasestructure.xml");
            DataRow toInsert = news.Tables[table].NewRow();
            DataRow toDB = database.Tables[table].NewRow();
            for (int i = 0; i < from.ItemArray.Length; i++ )
            {
                toInsert[i] = from[i];
                toDB[i] = from[i];
            }
            news.Tables[table].Rows.Add(toInsert);
            try
            {
                File.Delete("databaseinsert.xml");
            }
            catch (IOException ex) { }
            news.WriteXml("databaseinsert.xml");
            database.Tables[table].Rows.Add(toDB);
            try
            {
                File.Delete("database.xml");
            }
            catch (IOException ex) { }
            database.WriteXml("database.xml");
        }

        
    }
}
