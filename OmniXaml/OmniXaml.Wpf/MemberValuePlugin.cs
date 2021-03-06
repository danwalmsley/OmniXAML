namespace OmniXaml.Wpf
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using Typing;

    public class MemberValuePlugin : Typing.MemberValuePlugin
    {
        private readonly MutableXamlMember xamlMember;

        public MemberValuePlugin(MutableXamlMember xamlMember) : base(xamlMember)
        {
            this.xamlMember = xamlMember;
        }

        public override object GetValue(object instance)
        {
            return base.GetValue(instance);
        }

        public override void SetValue(object instance, object value)
        {
            if (!TrySetDependencyProperty(instance, value))
            {
                base.SetValue(instance, value);
            }                      
        }

        private bool TrySetDependencyProperty(object instance, object value)
        {
            var dp = GetDependencyProperty(instance.GetType(), xamlMember.Name + "Property");
            if (dp == null)
            {
                return false;
            }

            var dependencyObject = (DependencyObject) instance;
            if (value is BindingExpression)
            {
                dependencyObject.SetValue(dp, value);
            }
            else
            {
                dependencyObject.SetValue(dp, value);
            }
            return true;
        }

        private static DependencyProperty GetDependencyProperty(Type type, string name)
        {
            FieldInfo fieldInfo = type.GetField(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return (DependencyProperty) fieldInfo?.GetValue(null);
        }
    }
}