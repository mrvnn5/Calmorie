using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App6.Adapters
{
    public class InputFilter : InputFilterAllCaps
    {
        InputType inputType { get; set; }
        public InputFilter(InputType inputType) : base()
        {
            this.inputType = inputType;
        }

        override
        public ICharSequence FilterFormatted(ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
        {
            if(inputType == InputType.Username)
            {
                for (int i = start; i < end; i++)
                {
                    if (!Character.IsLetterOrDigit(source.CharAt(i)))
                    {
                        return CharSequence.ArrayFromStringArray(new[] { "" })[0];
                    }
                }
                return null;
            }

            return null;
        }

        public enum InputType
        {
            Username = 0
        }
    }
}