using Microsoft.AspNetCore.Mvc.ModelBinding;    
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace ApsnetCore.Persentation.ModelBinders
{ 
    public class ArrayModelBinder : IModelBinder
    {
        //The code you provided defines a custom model binder named `ModelBinders` that implements the `IModelBinder` interface. 
        //This model binder is responsible for binding an array of values from the request to a model property.
        //Let's break down the code:

        //- The `BindModelAsync` method is the main entry point for the model binder.
        //It takes a `ModelBindingContext` parameter and returns a `Task`.
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ///-The first check `if (!bindingContext.ModelMetadata.IsEnumerableType)` ensures that the model property being bound
            ///is an enumerable type(e.g., an array or a collection).
            ///If it's not, the model binding fails, and a `ModelBindingResult.Failed()` is assigned to the `bindingContext.Result` property.
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            ///-The next step is to retrieve the value of the model property from the value provider using
            ///the model name specified in the binding context.
            ///It's converted to a string using `ToString()`.
            var providedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            
            ///-If the provided value is empty or null, the model binding succeeds with a `null` value assigned to the 
            ///`bindingContext.Result`, indicating that the model property should be set to `null`.
            if (string.IsNullOrEmpty(providedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            /// -If a non-empty value is provided, the code determines the generic type of the model
            ///property by accessing the `GenericTypeArguments` property
            ///of the `ModelType` property of the binding context.This assumes(يفترض) that the model property
            ///is an array with a single generic argument.
            var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];

            ///-Then, a `TypeConverter` is obtained(مُقتَنىً - مكتسب) for the generic type using `TypeDescriptor.GetConverter(genericType)`. 
            ///This converter is used to convert each individual value in the provided value string to the appropriate(ملائم) type.
            var converter = TypeDescriptor.GetConverter(genericType);

            ///-The provided value string is split using a comma as the delimiter(محدد), and each trimmed value is converted to
            ///the generic type using the type converter.The resulting objects are stored in an object array.
            var objectArray = providedValue.Split(new[] { "," },
                StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim())).ToArray();

            ///-An array of the generic type is created with the same length as the object array.
            var guidArray = Array.CreateInstance(genericType, objectArray.Length);

            ///-The objects from the object array are copied to the newly created generic type array using `objectArray.CopyTo(guidArray, 0)`.
            objectArray.CopyTo(guidArray, 0);

            ///-Finally, the `bindingContext.Model` is set to the generic type array,
            ///and the `bindingContext.Result` is assigned a `ModelBindingResult.Success` with the model as the value.
            bindingContext.Model = guidArray;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            ///-The method returns `Task.CompletedTask` to signify the completion of the model binding process.
            ///This custom model binder can be registered in your application's startup code to handle
            ///    binding for specific model properties or types.
            return Task.CompletedTask;

        }
    }
}
