using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace mossrecru.Views.UserControls
{
    public partial class FilterItem : ContentView
    {
        public event EventHandler Tapped;
        public FilterItem()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
        }

        public int Value { get; set; }

        public string Value1 { get; set; }

        bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;

                if (value)
                {
                    Select();
                }
                else
                {
                    DeSelect();
                }
            }
        }

        private void On_Tapped(object sender, EventArgs e)
        {
            Select();
        }


        internal void Select()
        {
            isSelected = true;
            frm.BackgroundColor = Color.FromHex("#cb423f");
            Tapped?.Invoke(this, new EventArgs());
        }

        internal void DeSelect()
        {
            isSelected = false;
            frm.BackgroundColor = Color.Transparent;
        }
    }
}
