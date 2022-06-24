using BeautySalon.Commands;
using BeautySalon.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BeautySalon.ViewModels
{
    public class ServicesViewModel : INotifyPropertyChanged
    {
        public int AllCount { get; set; }
        public int SearchCount { get; set; }

        private ObservableCollection<Service> services;
        public ObservableCollection<Service> Services
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Filter))
                {
                    if (SelectedFilterProc == 0)
                    {
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Title.Contains(Filter)).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 1)
                    {
                        SearchCount = 0;

                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Title.Contains(Filter) && s.Discount < 0.05).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 2)
                    {
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Title.Contains(Filter) && s.Discount * 100 >= 5 && s.Discount * 100 < 15).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 3)
                    {
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Title.Contains(Filter) && s.Discount * 100 >= 15 && s.Discount * 100 < 30).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 4)
                    {
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Title.Contains(Filter) && s.Discount * 100 >= 30 && s.Discount * 100 < 70).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 5)
                    {
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Title.Contains(Filter) && s.Discount * 100 >= 70 && s.Discount * 100 < 100).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }

                    return services;
                }
                else
                {

                    if (SelectedFilterProc == 0)
                    {
                        AllCount = 0;
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.ToList());
                        AllCount = services.Count;
                        SearchCount = services.Count;
                        OnPropertyChanged("AllCount");
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 1)
                    {
                        SearchCount = 0;
                        
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s=>s.Discount < 0.05).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 2)
                    {
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Discount * 100 >= 5 && s.Discount*100 < 15).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 3)
                    {
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Discount * 100 >= 15 && s.Discount * 100 < 30).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 4)
                    {
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Discount * 100 >= 30 && s.Discount * 100 < 70).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    else if (SelectedFilterProc == 5)
                    {
                        SearchCount = 0;
                        services = new ObservableCollection<Service>(Connection.DataBase.Service.Where(s => s.Discount * 100 >= 70 && s.Discount * 100 < 100).ToList());

                        SearchCount = services.Count;
                        OnPropertyChanged("SearchCount");
                    }
                    return services;
                }
            }
            set
            {
                services = value;
                OnPropertyChanged("Services");
            }
        }

        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged("Filter");
                OnPropertyChanged("Services");
            }
        }

        private int selectedFilterProc;
        public int SelectedFilterProc 
        { 
            get { return selectedFilterProc; }
            set
            {
                selectedFilterProc = value;
                OnPropertyChanged("SelectedFilterProc");
                OnPropertyChanged("Services");
            }
        }
        private ObservableCollection<ClientService> zapisi;
        public ObservableCollection<ClientService> Zapisi
        {
            get
            {
                zapisi = new ObservableCollection<ClientService>(Connection.DataBase.ClientService/*.Where(cs => cs.StartTime == DateTime.Now && cs.StartTime == DateTime.Now)*/.ToList());
                return zapisi;
            }
            set { zapisi = value; OnPropertyChanged("Zapisi"); }
        }

        public void AdminAuthorization(Window wnd)
        {
            if (Pswrd == "0000")
            {
                visibility = true;
                Item.IsEnabled = true;
                wnd.Close();
                OnPropertyChanged("Item");
                OnPropertyChanged("Visib");
            }
        }

        public void ShowFunc()
        {
            visibilityList = true;
            OnPropertyChanged("VisibList");
        }

        public void DelServFunc(Service serv)
        {
            var result = MessageBox.Show($"Вы действительно хотите удалить услугу {serv.Title}?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var zapisi = Connection.DataBase.ClientService.Where(cs => cs.ServiceID == serv.ID).ToList();
                if (!zapisi.Any())
                {
                    Connection.DataBase.Service.Remove(serv);
                    Connection.DataBase.SaveChanges();
                    OnPropertyChanged("Services");
                }
                else
                {
                    MessageBox.Show("Вы не можете удалить данную услугу, т.к. имеются записи на данную услугу", "Информация", MessageBoxButton.OK);
                }
            }
        }

        public string Source { get; set; }
        public void AddPhotoFunc()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
                               // dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
         "Portable Network Graphic (*.png)|*.png";   // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                /*new BitmapImage(new Uri(dlg.FileName));*/
                string[] paths = dlg.FileName.Split('\\');
                Source = "\\Images\\" + paths[paths.Length - 1];
                if (!Connection.DataBase.Service.Where(s => s.MainImagePath == Source).Any())
                {
                    try
                    {

                        string query = "Действительно переместить файл \n" + dlg.FileName + " ?";
                        if (MessageBox.Show(query, "Переместить файл?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string applicationDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                            string rootpath = Directory.GetParent(applicationDirectory).Parent.FullName + "\\Images\\";

                            File.Copy(dlg.FileName, rootpath + Path.GetFileName(dlg.FileName), true);


                            //var oldpath = "";

                            //if (SelectServ.MainImagePath != null)
                            //{
                            //    oldpath = SelectServ.MainImagePath;
                            //}

                            SelectServ.MainImagePath = Source;
                            Connection.DataBase.SaveChanges();

                            //if (oldpath != null)
                            //{
                            //    if (!string.IsNullOrWhiteSpace(oldpath))
                            //    {
                            //        File.Delete(oldpath);
                            //    }
                            //}

                            OnPropertyChanged("Services");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удается переместить файл из-за исключения: " + ex.Message);
                    }
                }
                else
                {
                    SelectServ.MainImagePath = Source;
                    Connection.DataBase.SaveChanges();
                    OnPropertyChanged("Services");
                }
            }
        }


        public void AddPhotoFuncE()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
                               // dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
         "Portable Network Graphic (*.png)|*.png";   // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                /*new BitmapImage(new Uri(dlg.FileName));*/
                string[] paths = dlg.FileName.Split('\\');
                Source = "\\Images\\" + paths[paths.Length - 1];
                if (!Connection.DataBase.Service.Where(s => s.MainImagePath == Source).Any())
                {
                    try
                    {

                        string query = "Действительно переместить файл \n" + dlg.FileName + " ?";
                        if (MessageBox.Show(query, "Переместить файл?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string applicationDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                            string rootpath = Directory.GetParent(applicationDirectory).Parent.FullName + "\\Images\\";

                            File.Copy(dlg.FileName, rootpath + Path.GetFileName(dlg.FileName), true);

                            SMainImagePath = Source;

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удается переместить файл из-за исключения: " + ex.Message);
                    }
                }
                else
                {
                    smainImagePath = Source;
                }
            }
        }

        private string stitle;
        public string STitle
        {
            get
            {
                return stitle;
            }
            set
            {
                stitle = value;
                OnPropertyChanged("STitle");
            }
        }

        private decimal scost;
        public decimal SCost
        {
            get
            {
                return scost;
            }
            set
            {
                scost = value;
                OnPropertyChanged("SCost");
            }
        }

        private int sdurationInSeconds;
        public int SDurationInSeconds
        {
            get
            {
                return sdurationInSeconds;
            }
            set
            {
                sdurationInSeconds = value;
                OnPropertyChanged("SDurationInSeconds");
            }
        }

        private string sdescription;
        public string SDescription
        {
            get { return sdescription; }
            set { sdescription = value; OnPropertyChanged("SDescription"); }
        }

        private double sdiscount;
        public double SDiscount
        {
            get { return sdiscount; }
            set { sdiscount = value; OnPropertyChanged("SDiscount"); }
        }

        private string smainImagePath;
        public string SMainImagePath
        {
            get { return smainImagePath; }
            set { smainImagePath = value; OnPropertyChanged("SMainImagePath"); }
        }

        public void AddServFunc(Window wnd)
        {
            StringBuilder errors = new StringBuilder();

            try
            {
                if (string.IsNullOrWhiteSpace(STitle))
                    errors.AppendLine("Дайте название услуге");
                if (Connection.DataBase.Service.Where(s => s.Title == STitle).Any())
                {
                    errors.AppendLine("Данное название услуги уже существует!");
                }
                if (SCost <= 0)
                    errors.AppendLine("Укажите корректную стоимость");
                if (SDurationInSeconds <= 0 || SDurationInSeconds > 300)
                    errors.AppendLine("Укажите корректное время выполнения услуги");


                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }

                if (SDiscount != 0)
                {
                    NewServ = new Service()
                    {
                        Title = STitle,
                        Cost = SCost,
                        DurationInSeconds = Convert.ToInt32(SDurationInSeconds) * 60,
                        Description = SDescription,
                        Discount = SDiscount / 100,
                        MainImagePath = SMainImagePath
                    };
                }
                else
                {
                    NewServ = new Service()
                    {
                        Title = STitle,
                        Cost = SCost,
                        DurationInSeconds = Convert.ToInt32(SDurationInSeconds) * 60,
                        Description = SDescription,
                        Discount = SDiscount,
                        MainImagePath = SMainImagePath
                    };
                }


                Connection.DataBase.Service.Add(NewServ);
                Connection.DataBase.SaveChanges();
                MessageBox.Show("Информация сохранена");
                OnPropertyChanged("Services");
                wnd.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void SaveChangesFunc()
        {
            Connection.DataBase.SaveChanges();

            OnPropertyChanged("Services");
        }

        private Service selectedServ;
        public Service SelectServ
        {
            get
            {
                return selectedServ;
            }
            set
            {
                selectedServ = value;
                OnPropertyChanged("SelectServ");
            }
        }

        private ObservableCollection<Client> clientsFIO;
        public ObservableCollection<Client> ClientsFIO
        {
            get
            {
                clientsFIO = new ObservableCollection<Client>(Connection.DataBase.Client.ToList());
                return clientsFIO;
            }
            set
            {
                clientsFIO = value;
                OnPropertyChanged("ClientsFIO");
            }
        }

        private Client selectedClient;
        public Client SelectedClient
        {
            get
            {
                return selectedClient;
            }
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;

                OnPropertyChanged("Date");
            }
        }

        private string time;
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }

        public void AddZapisFunc(Window wnd)
        {
            StringBuilder errors = new StringBuilder();

            if (SelectedClient == null)
                errors.AppendLine("Выберите клиента");
            if (Date < DateTime.Now.Date)
                errors.AppendLine("Введите корректную дату оказания услуги");
            if (string.IsNullOrEmpty(Time))
                errors.AppendLine("Укажите время оказания услуги");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            string[] mass = Time.Split(':');
            date = date.AddHours(0);
            date = date.AddMinutes(0);
            date = date.AddHours(int.Parse(mass[0]));
            date = date.AddMinutes(int.Parse(mass[1]));
            OnPropertyChanged("Date");

            NewZapis = new ClientService()
            {
                ServiceID = SelectServ.ID,
                ClientID = SelectedClient.ID,
                StartTime = Date,
            };

            Connection.DataBase.ClientService.Add(NewZapis);
            Connection.DataBase.SaveChanges();
            OnPropertyChanged("Zapisi");

            wnd.Close();
        }

        public MenuItem Item { get; set; }
        public string Pswrd { get; set; }
        public RelayCommand OpenAdmin { get; set; }
        public RelayCommand AdminActive { get; set; }
        public RelayCommand DelService { get; set; }
        public RelayCommand OpenEditServW { get; set; }
        public RelayCommand SaveChanges { get; set; }
        public RelayCommand AddPhoto { get; set; }
        public RelayCommand DeletePhoto { get; set; }
        public RelayCommand OpenAddServW { get; set; }
        public RelayCommand AddServ { get; set; }
        public Service NewServ { get; set; }
        public RelayCommand AddPhotoE { get; set; }
        public RelayCommand DeletePhotoE { get; set; }
        public RelayCommand OpenZapisi { get; set; }
        public RelayCommand OpenAddZapisW { get; set; }
        public RelayCommand AddZapis { get; set; }
        public ClientService NewZapis { get; set; }
        public RelayCommand CloseWND { get; set; }
        public RelayCommand ShowList { get; set; }
        public ServicesViewModel()
        {
            Connection.DataBase = new SalonBDEntities();
            OnPropertyChanged("Services");
            visibility = false;

            OpenAdmin = new RelayCommand(param =>
            {
                AdminWindow adminWindow = new AdminWindow();
                Item = param as MenuItem;
                adminWindow.ShowDialog();
            },
            param =>
            {
                return param != null;
            });

            AdminActive = new RelayCommand(param =>
            {
                AdminAuthorization(param as AdminWindow);
            },
           param =>
           {
               return param != null;
           });

            DelService = new RelayCommand(param =>
            {
                DelServFunc(param as Service);
            },
           param =>
           {
               return param != null;
           });

            OpenEditServW = new RelayCommand(param =>
            {
                RenameWindow renameWindow = new RenameWindow();
                SelectServ = param as Service;
                OnPropertyChanged("SelectServ");
                renameWindow.ShowDialog();
            },
           param =>
           {
               return param != null;
           });

            AddPhoto = new RelayCommand(param =>
            {
                AddPhotoFunc();
            },
          param =>
          {
              return param == null;
          });

            SaveChanges = new RelayCommand(param =>
            {
                SaveChangesFunc();
            },
           param =>
           {
               return param == null;
           });

            OpenAddServW = new RelayCommand(param =>
            {
                AddServiceWindow addService = new AddServiceWindow();
                addService.ShowDialog();
            },
           param =>
           {
               return param == null;
           });

            AddServ = new RelayCommand(param =>
            {
                AddServFunc(param as AddServiceWindow);
            },
           param =>
           {
               return param != null;
           });

            AddPhotoE = new RelayCommand(param =>
            {
                AddPhotoFuncE();
            },
             param =>
             {
                 return param == null;
             });

            OpenZapisi = new RelayCommand(param =>
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
            },
           param =>
           {
               return param == null;
           });

            OpenAddZapisW = new RelayCommand(param =>
            {
                AddZapisWindow addZapis = new AddZapisWindow();
                addZapis.ShowDialog();
            },
           param =>
           {
               return param != null;
           });

            AddZapis = new RelayCommand(param =>
            {
                AddZapisFunc(param as AddZapisWindow);
            },
           param =>
           {
               return param != null;
           });

            CloseWND = new RelayCommand(param =>
            {
                var wnd = param as RenameWindow;
                wnd.Close();
            },
            param =>
            {
                return param != null;
            });

            ShowList = new RelayCommand(param =>
            {
                ShowFunc();
                
            },
            param =>
            {
              return param == null;
            });

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OnPropertyChanged("Zapisi");
        }

        private bool visibility;
        public bool Visib
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                OnPropertyChanged("Visib");
            }
        }

        private bool visibilityList;
        public bool VisibList
        {
            get
            {
                return visibilityList;
            }
            set
            {
                visibilityList = value;
                OnPropertyChanged("VisibList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
