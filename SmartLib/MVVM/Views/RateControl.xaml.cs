using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace J4ni.Controls
{
    public partial class RateControl : UserControl
    {
        private List<CheckBox> stars = new List<CheckBox>();
        
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(int), typeof(RateControl), new PropertyMetadata(0, OnScaleChanged));
        public static readonly DependencyProperty RatingProperty = DependencyProperty.Register("Rating", typeof(int), typeof(RateControl), new PropertyMetadata(0, OnRatingSet));

        public int Scale
        {
            get
            {
                return (int)GetValue(ScaleProperty);
            }
            set
            {
                SetValue(ScaleProperty, value);
            }
        }

        public int Rating
        {
            get
            {
                return (int)GetValue(RatingProperty);
            }
            set
            {
                SetValue(RatingProperty, value);
            }
        }

        private bool initDone = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public RateControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(OnLoaded);
            this.SizeChanged += new SizeChangedEventHandler(OnResized);
        }

        /// <summary>
        /// Now is safe to create the stars, we have measurements available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            initDone = true;

            CreateStars(this, Scale);
            Rating = tempRating;
        }

        /// <summary>
        /// Handle the resizing of the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResized(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width > 0
                && e.PreviousSize.Height > 0)
            {
                RateControl rat = sender as RateControl;

                GridScale.ScaleX = e.NewSize.Width / e.PreviousSize.Width;
                GridScale.ScaleY = e.NewSize.Height / e.PreviousSize.Height;
            }
        }

        private int tempRating = 0;
        /// <summary>
        /// Rate was set for the control
        /// </summary>
        /// <param name="obj">Reference to the instance of this class</param>
        /// <param name="args">Value that the user set</param>
        static void OnRatingSet(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RateControl rat = obj as RateControl;

            if (rat != null
                && rat.initDone == true)
            {
                int? amount = args.NewValue as int?;

                if (amount != null && amount <= rat.Scale && amount >= 0)
                {
                    if (amount > 0)
                    {
                        SetStars(rat.stars[(int)amount - 1], rat);
                    }
                    else
                    {
                        if (rat.Scale > 0)
                        {
                            SetStars(null, rat);
                        }
                    }
                }
            }
            else
            {
                rat.tempRating = (args.NewValue as int?) ?? 0;
            }
        }

        /// <summary>
        /// User changed the number of stars
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        static void OnScaleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RateControl rat = obj as RateControl;

            if (rat != null && rat.initDone == true)
            {
                int? amount = args.NewValue as int?;

                if (amount != null)
                {
                    CreateStars(rat, amount);
                }
            }
        }

        private static void CreateStars(RateControl rat, int? amount)
        {
            if (amount == null)
            {
                amount = 1;
            }
            
            // Do the cleanup for the old stuff
            foreach (CheckBox box in rat.stars)
            {
                rat.LayoutRoot.Children.Remove(box);
            }
            rat.stars.Clear();
            List<ColumnDefinition> columnDefinitionsToRemove = new List<ColumnDefinition>();
            foreach (ColumnDefinition def in rat.LayoutRoot.ColumnDefinitions)
            {
                columnDefinitionsToRemove.Add(def);
            }

            foreach (ColumnDefinition def in columnDefinitionsToRemove)
            {
                rat.LayoutRoot.ColumnDefinitions.Remove(def);
            }
            
            rat.Rating = 0;
            
            // Create the new controls
            for (int i = 0; i < amount; ++i)
            {
                rat.LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
                CheckBox check = new CheckBox();
                check.Name = "check" + i.ToString();
                check.SetValue(Grid.ColumnProperty, i);

                check.HorizontalAlignment = HorizontalAlignment.Left;
                check.Checked += new RoutedEventHandler(OnCheck);
                check.Unchecked += new RoutedEventHandler(OnCheck);
                rat.stars.Add(check);
                rat.LayoutRoot.Children.Add(check);
            }
        }

        private static bool checkingInProgress = false;

        /// <summary>
        /// User clicks on the star
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCheck(object sender, RoutedEventArgs e)
        {
            if (checkingInProgress == true)
            {
                return;
            }

            CheckBox check = sender as CheckBox;

            if (check != null)
            {
                RateControl rat = ((Grid)check.Parent).Parent as RateControl;

                SetStars(check, rat);
            }
        }

        /// <summary>
        /// Set the visual state for the stars
        /// </summary>
        /// <param name="check">User selected star</param>
        /// <param name="rat">reference to the instance of this control</param>
        private static void SetStars(CheckBox check, RateControl rat)
        {
            if (rat != null)
            {
                checkingInProgress = true;

                bool found = false;
                
                // we want to empty the selection, so cheat the check routine
                if (check == null)
                {
                    found = true;
                }

                // Loop through the stars and light them on if they are 
                // below selected and dim if they are above the selected
                for (int i = 0; i < rat.stars.Count; i++)
                {
                    if (check != rat.stars[i])
                    {
                        if (!found)
                        {
                            rat.stars[i].IsChecked = true;
                        }
                        else
                        {
                            rat.stars[i].IsChecked = false;
                        }
                    }
                    else
                    {
                        found = true;
                        rat.stars[i].IsChecked = true;
                        rat.Rating = i + 1;
                    }
                }
                checkingInProgress = false;
            }
        }
    }
}
