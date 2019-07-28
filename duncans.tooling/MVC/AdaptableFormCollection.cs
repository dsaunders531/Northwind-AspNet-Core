// <copyright file="AdaptableFormCollection.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Collections.Generic;

namespace duncans.MVC
{
    public class AdaptableFormCollection : IFormCollection
    {
        public AdaptableFormCollection()
        {
            this.Values = new List<KeyValuePair<string, StringValues>>();
            this.FileValues = new FormFileCollection();
        }

        public AdaptableFormCollection(IFormCollection form)
        {
            this.Values = new List<KeyValuePair<string, StringValues>>();

            foreach (string key in form.Keys)
            {
                this.Values.Add(new KeyValuePair<string, StringValues>(key, form[key]));
            }

            this.FileValues = new FormFileCollection();

            foreach (IFormFile item in form.Files)
            {
                this.FileValues.Add(item);
            }
        }

        public int Count => this.Values.Count;

        public ICollection<string> Keys
        {
            get
            {
                List<string> result = new List<string>();

                foreach (KeyValuePair<string, StringValues> value in this.Values)
                {
                    result.Add(value.Key);
                }

                return result;
            }
        }

        public IFormFileCollection Files => this.FileValues;

        private List<KeyValuePair<string, StringValues>> Values { get; set; }

        private FormFileCollection FileValues { get; set; }

        public StringValues this[string key]
        {
            get
            {
                StringValues result = default(StringValues);
                this.TryGetValue(key, out result);
                return result;
            }
        }

        public bool ContainsKey(string key)
        {
            bool result = false;

            foreach (KeyValuePair<string, StringValues> value in this.Values)
            {
                result = value.Key == key;

                if (result)
                {
                    break;
                }
            }

            return result;
        }

        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public bool TryGetValue(string key, out StringValues value)
        {
            bool result = false;

            if (ContainsKey(key))
            {
                foreach (KeyValuePair<string, StringValues> item in this.Values)
                {
                    if (item.Key == key)
                    {
                        result = true;
                        value = item.Value;
                    }

                    if (result)
                    {
                        break;
                    }
                }
            }
            else
            {
                value = default(StringValues);
            }

            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public void Remove(string key)
        {
            List<KeyValuePair<string, StringValues>> values = this.Values; // items are to be deleted so this is needed.

            foreach (KeyValuePair<string, StringValues> item in values)
            {
                if (item.Key == key)
                {
                    this.Values.Remove(item);
                    break;
                }
            }
        }

        public void Add(string key, StringValues value)
        {
            this.Add(new KeyValuePair<string, StringValues>(key, value));
        }

        public void Add(KeyValuePair<string, StringValues> value)
        {
            this.Values.Add(value);
        }
    }
}
