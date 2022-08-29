using kiosk_solution.Data.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Utilities
{
    public static class LinQUtils
    {
        public static IQueryable<TEntity> DynamicFilter<TEntity>(this IQueryable<TEntity> source, TEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            foreach (var item in properties)
            {
                if (entity.GetType().GetProperty(item.Name) == null) continue;
                var propertyVal = entity.GetType().GetProperty(item.Name).GetValue(entity, null);
                if (propertyVal == null) continue;
                if (item.CustomAttributes.Any(a => a.AttributeType == typeof(SkipAttribute))) continue;
                bool isDateTime = item.PropertyType == typeof(DateTime);
                if (isDateTime)
                {
                    DateTime dt = (DateTime) propertyVal;
                    source = source.Where($"{item.Name} >= @0 && {item.Name} < @1", dt.Date, dt.Date.AddDays(1));
                }
                else if (item.CustomAttributes.Any(a => a.AttributeType == typeof(ContainAttribute)))
                {
                    var array = (IList) propertyVal;
                    source = source.Where($"{item.Name}.Any(a=> @0.Contains(a))", array);
                }
                else if (item.CustomAttributes.Any(a => a.AttributeType == typeof(StringAttribute)) && (item.Name.Equals("Status") || (item.Name.Equals("ServiceApplicationStatus"))))
                {
                    if(source == null)
                    {

                    }
                    else
                    {
                        source = source.Where($"{item.Name}.ToLower().Equals(@0)", propertyVal.ToString().ToLower());
                    }
                  
                }
                else if (item.CustomAttributes.Any(a => a.AttributeType == typeof(StringAttribute)))
                {
                    source = source.Where($"{item.Name}.ToLower().Contains(@0)", propertyVal.ToString().ToLower());
                }
                else if (item.PropertyType == typeof(string))
                {
                    source = source.Where($"{item.Name} = \"{((string) propertyVal).Trim()}\"");
                }
                else if (item.CustomAttributes.Any(a => a.AttributeType == typeof(GuidAttribute)))
                {
                    source = source.Where($"{item.Name} == \"{((Guid) propertyVal)}\"");
                }
                else if (item.CustomAttributes.Any(a => a.AttributeType == typeof(SpecificAttribute)))
                {      

                }
                else
                {
                    source = source.Where($"{item.Name} = {propertyVal}");
                }
            }

            return source;
        }

        public static PagingViewModel<TResult> PagingIQueryable<TResult>(this IQueryable<TResult> source, int page,
            int size, int limitPaging, int defaultPaging)
        {
            if (size > limitPaging) size = limitPaging;
            if (size < 1) size = defaultPaging;
            if (page < 1) page = 1;
            int total = source.Count();
            IQueryable<TResult> results = source.Skip((page - 1) * size)
                .Take(size);

            return new PagingViewModel<TResult>()
            {
                Total = total,
                Data = results
            };
        }
    }
}