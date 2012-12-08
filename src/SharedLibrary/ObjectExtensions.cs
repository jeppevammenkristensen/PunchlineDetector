using System;

namespace SharedLibrary
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// This will throw an exception if object cannot convert
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TObject As<TObject>(this object source) where TObject : class 
        {
            var result = source as TObject;
            if (result == null)
                throw new InvalidOperationException(string.Format("Could not convert the inputted object to type {0}",
                                                                  typeof (TObject)));
            
            return result;

        }
    }
}