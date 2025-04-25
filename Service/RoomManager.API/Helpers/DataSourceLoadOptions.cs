﻿using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RoomManager.API.Helpers
{
    [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
    public class DataSourceLoadOptions : DataSourceLoadOptionsBase
    {
    }

    public class DataSourceLoadOptionsBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            DataSourceLoadOptions loadOptions = new DataSourceLoadOptions();
            DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
            bindingContext.Result = ModelBindingResult.Success(loadOptions);
            return Task.CompletedTask;
        }
    }
}
