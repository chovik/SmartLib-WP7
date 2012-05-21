using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;

namespace Infrastructure.Validation
{
    [DataContract]
    public class ValidationViewModel : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        public ValidationViewModel()
        {
            this.PropertyChanged += (s, e) =>
            {
                // if the changed property is one of the properties which require validation
                if (this.Validator.PropertyNames != null
                    && this.Validator.PropertyNames.Contains(e.PropertyName))
                {
                    this.Validator.ValidateProperty(e.PropertyName);
                    this.OnErrorsChanged(e.PropertyName);
                }
            };
        }

        protected ModelValidator validator = new ModelValidator();

        public ModelValidator Validator 
        {
            get
            {
                return validator ?? new ModelValidator();
            }
            set
            {
                validator = value;
            }
        }

        public bool ValidateAll()
        {
            var result = this.validator.ValidateAll();

            this.Validator.PropertyNames.ToList().ForEach(this.OnErrorsChanged);

            return result;
        }

        #region INotifyDataErrorInfo

        public IEnumerable GetErrors(string propertyName)
        {
            return this.Validator.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get { return this.Validator.ErrorMessages.Count > 0; }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        protected virtual void OnErrorsChanged(string propertyName)
        {
            this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));

            this.OnNotifyPropertyChanged("HasErrors");
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnNotifyPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
