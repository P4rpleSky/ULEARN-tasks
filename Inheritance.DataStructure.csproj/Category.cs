using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public string Product { get; set; }
        public MessageType Type { get; set; }
        public MessageTopic Topic { get; set; }

        public Category(string product, MessageType messageType, MessageTopic messageTopic)
        {
            this.Product = product is null? "null" : product;
            this.Type = messageType;
            this.Topic = messageTopic;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Category)) return int.MinValue;
            var category = obj as Category;
            var productComparison = this.Product.CompareTo(category.Product);
            var typeComparison = this.Type.CompareTo(category.Type);
            var topicComparison = this.Topic.CompareTo(category.Topic);
            if (productComparison != 0)
                return productComparison;
            if (typeComparison != 0)
                return typeComparison;
            if (topicComparison != 0)
                return topicComparison;
            return 0;
        }

        public override string ToString()
        {
            return this.Product + "." + this.Type + "." + this.Topic;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Category)) return false;
            var category = obj as Category;
            return this.Product.Equals(category.Product) &&
                   this.Type == category.Type &&
                   this.Topic == category.Topic;
        }

        public override int GetHashCode()
        {
            return this.Product.GetHashCode() + 8 * (int)this.Type + 99 * (int)this.Topic;
        }

        public static bool operator >(Category category1, Category category2)
        {
            return category1.CompareTo(category2) > 0 ? true : false;
        }

        public static bool operator <(Category category1, Category category2)
        {
            return category1.CompareTo(category2) < 0 ? true : false;
        }

        public static bool operator >=(Category category1, Category category2)
        {
            return category1.CompareTo(category2) >= 0 ? true : false;
        }

        public static bool operator <=(Category category1, Category category2)
        {
            return category1.CompareTo(category2) <= 0 ? true : false;
        }
    }
}