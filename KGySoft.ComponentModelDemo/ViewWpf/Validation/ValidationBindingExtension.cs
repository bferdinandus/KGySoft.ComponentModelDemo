﻿#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

using KGySoft.ComponentModel;

#endregion

namespace KGySoft.ComponentModelDemo.ViewWpf.Validation
{
    // Use this markup extension to get the Error, Warning or Information validation message for an IValidatingObject.
    // See the MainWindow.xaml for examples and more description.
    /// <summary>
    /// A markup extension and a converter to get a validation message for the specified severity from a <see cref="KGySoft.ComponentModel.ValidationResult"/>
    /// for an <see cref="IValidatingObject"/> instance.
    /// </summary>
    public class ValidationResultExtension : MarkupExtension, IValueConverter
    {
        #region MultiBindingConverter class

        // When BoundTarget property is specified this IMultiValueConverter is created internally to be able to resolve both the binding to the object to validate
        // and BoundTarget, which is the object (typically a control) to which the IValidatingObject is bound.
        private sealed class MultiBindingConverter : IMultiValueConverter
        {
            #region Fields

            private readonly ValidationResultExtension owner;

            #endregion

            #region Constructors

            public MultiBindingConverter(ValidationResultExtension owner) => this.owner = owner;

            #endregion

            #region Methods

            #region Public Methods

            /// <summary>
            /// This method is executed when <see cref="ValidationResultExtension"/> is used in a binding position and <see cref="BoundTarget"/> is not <see langword="null"/>.
            /// </summary>
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                if (owner.BoundTarget == null || values == null || values.Length != 2)
                    return null;

                // values[0] is the object to validate
                // values[1] is the resolved value of BoundTarget

                // Resolved BoundTarget is not a dependency object: we cannot tell the property name.
                // In this case we allow returning any result if no properties were specified
                if (!(values[1] is DependencyObject target))
                    return owner.PropertyName == null && owner.BoundProperty == null ? owner.Convert(values[0], targetType, null, culture) : null;

                // 1. Bound property is explicitly defined
                var boundProperty = owner.BoundProperty;

                // 2. by name
                if (boundProperty == null && owner.PropertyName != null)
                {
                    Type type = target.GetType();
                    var descriptor = DependencyPropertyDescriptor.FromName(owner.PropertyName, type, type);
                    boundProperty = descriptor?.DependencyProperty;
                }

                Binding targetBinding = null;
                if (boundProperty != null)
                    targetBinding = BindingOperations.GetBinding(target, boundProperty);

                // 3. no property was defined, determining by first bound property
                if (boundProperty == null && owner.PropertyName == null)
                {
                    foreach (DependencyProperty dependencyProperty in EnumerateDependencyProperties(target))
                    {
                        targetBinding = BindingOperations.GetBinding(target, dependencyProperty);
                        if (((object)targetBinding?.Path ?? targetBinding?.XPath) != null)
                            break;
                    }
                }

                string path = targetBinding?.Path?.Path ?? targetBinding?.XPath;
                string propertyName = path?.Split(new[] { '.', '/', '[', ']', '(', ')' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

                // If property name was defined but no such property exists or path could not be parsed we don't return any validation.
                // But if there was no property defined and we did not find a bound property returning a result for any property.
                if (propertyName == null && (owner.PropertyName != null || owner.BoundProperty != null))
                    return null;

                // After resolving some propertyName, eventually calls the outer IValueConverter.Convert.
                // Passing the found property name as parameter so it will not overwrite the owner's PropertyName
                return owner.Convert(values[0], targetType, propertyName, culture);
            }

            #endregion

            #region Explicitly Implemented Interface Methods

            object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();

            #endregion

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the severity of the message to return. If <see langword="null"/>, then a result of any severity can be returned.
        /// </summary>
        public ValidationSeverity? Severity { get; set; }

        /// <summary>
        /// Gets or sets the path of the validation result.
        /// <br/>Should point to an <see cref="IValidatingObject"/> or <see cref="IValidatingObject.ValidationResults"/> instance.
        /// <br/>If used in triggers the latter is preferred because changes of the ValidationResults property invoke the trigger.
        /// </summary>
        public PropertyPath Path { get; set; }

        /// <summary>
        /// Gets or sets the source for <see cref="Path"/>. If not set, the default data context is used.
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// If <see cref="BoundTarget"/> is <see langword="null"/>, then gets or sets the name of the validated property of the checked <see cref="IValidatingObject"/>.
        /// <br/>If <see cref="BoundTarget"/> is not <see langword="null"/>&#160;and <see cref="BoundProperty"/> is <see langword="null"/>, then gets or sets the name of the name of the bound property on the <see cref="BoundTarget"/> (eg. <see cref="TextBox.Text"/>), to be used to determine the name of the validated property.
        /// <br/>If both <see cref="BoundTarget"/> and <see cref="BoundProperty"/> are set, then this property is ignored.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets a binding to the bound target element, from which the name of the name of the validated property has to be determined.
        /// <br/>If this property is <see langword="null"/>, then <see cref="PropertyName"/> denotes the validated property of the checked <see cref="IValidatingObject"/> property.
        /// <br/>If this property is not <see langword="null"/>, then either <see cref="BoundProperty"/> or <see cref="PropertyName"/> denotes a property of the resulting object of this binding (eg. <see cref="TextBox.Text"/>.
        /// </summary>
        public BindingBase BoundTarget { get; set; }

        /// <summary>
        /// Gets or sets the bound property on <see cref="BoundTarget"/> (eg. <see cref="TextBox.TextProperty"/>) to be used to determine the name of the looked up property of the checked <see cref="IValidatingObject"/>.
        /// <br/>If <see cref="BoundTarget"/> is null, then this property is ignored.
        /// </summary>
        public DependencyProperty BoundProperty { get; set; }

        #endregion

        #region Constructors

        public ValidationResultExtension() { }
        public ValidationResultExtension(ValidationSeverity severity) => Severity = severity;

        #endregion

        #region Methods

        #region Static Methods

        private static IEnumerable<DependencyProperty> EnumerateDependencyProperties(DependencyObject obj)
        {
            if (obj != null)
            {
                LocalValueEnumerator lve = obj.GetLocalValueEnumerator();
                while (lve.MoveNext())
                {
                    yield return lve.Current.Property;
                }
            }
        }

        #endregion

        #region Instance Methods

        #region Public Methods

        // Gets the markup extension result. Can be used in binding and <see cref="IValueConverter"/> positions.
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target))
                return null;

            var pi = target.TargetProperty as PropertyInfo;
            var dp = target.TargetProperty as DependencyProperty;
            if (pi == null && dp == null)
                throw new NotSupportedException($"TargetProperty {target.TargetProperty?.GetType().Name} is not supported.");

            var propertyType = pi?.PropertyType ?? dp.PropertyType;

            // Converter (or object) expected
            if (typeof(IValueConverter).IsAssignableFrom(propertyType))
                return this;

            // Dependency or binding expected
            if (dp != null || typeof(BindingBase).IsAssignableFrom(propertyType))
            {
                Binding binding = new Binding { Path = Path };
                if (Source != null)
                    binding.Source = Source;

                // Simple binding with no bound target
                if (BoundTarget == null)
                {
                    binding.Converter = this;
                    binding.ConverterParameter = PropertyName; // the only effect here is to override a user-defined parameter
                    return binding;
                }

                // We need to evaluate two bindings so creating a multi binding
                return new MultiBinding
                {
                    Bindings = { binding, BoundTarget },
                    Converter = new MultiBindingConverter(this)
                };
            }

            throw new NotSupportedException($"Target type {pi.PropertyType} is not supported");
        }

        /// <summary>
        /// Gets the validation result message from an <see cref="IValidatingObject"/>.
        /// </summary>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var validationResults = value as ValidationResultsCollection ?? (value as IValidatingObject)?.ValidationResults;
            if (validationResults == null)
                return null;

            // parameter is set, when this Convert is called from the MultiBindingConverter.Convert (that is, when also BoundProperty had to be resolved)
            string validatedPropertyName = parameter as string ?? PropertyName;
            IEnumerable<string> messages = validationResults.Where(vr =>
                        (Severity == null || vr.Severity == Severity)
                        && (validatedPropertyName == null || vr.PropertyName == validatedPropertyName)).Select(vr => vr.Message);

            return messages.FirstOrDefault();
        }

        #endregion

        #region Explicitly Implemented Interface Methods

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();

        #endregion

        #endregion

        #endregion
    }
}
