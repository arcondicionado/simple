﻿using NUnit.Framework;
using SharpTestsEx;
using Simple.Expressions;
using Simple.IO.Serialization;
using Simple.Tests.Resources;

namespace Simple.Tests.Entities
{
    public class FetchFixture
    {
        [Test]
        public void CanFetchByFirstColumn()
        {
            var spec = Customer.Do.Fetch(x => x.Address);
            var queryable = new EmptyQueryable<Customer>("q").ApplySpecs(spec);
            queryable.Expression.ToString().Should().Be("q.Fetch(x => x.Address)");
        }

        [Test]
        public void CanFetchManyByFirstColumn()
        {
            var spec = Category.Do.FetchMany(x => x.Products);
            var queryable = new EmptyQueryable<Category>("q").ApplySpecs(spec);
            queryable.Expression.ToString().Should().Be("q.FetchMany(x => x.Products)");
        }

        [Test]
        public void CanFetchTwoColumns()
        {
            var spec = Customer.Do.Fetch(x => x.Address).Fetch(x => x.ContactName);
            var queryable = new EmptyQueryable<Customer>("q").ApplySpecs(spec);
            Assert.AreEqual("q.Fetch(x => x.Address).Fetch(x => x.ContactName)",
                queryable.Expression.ToString());
        }

        [Test]
        public void CanFetchThenTwoColumns()
        {
            var spec = Product.Do.Fetch(x => x.Category).ThenFetch(x => x.Name);
            var queryable = new EmptyQueryable<Product>("q").ApplySpecs(spec);
            Assert.AreEqual("q.Fetch(x => x.Category).ThenFetch(x => x.Name)",
                queryable.Expression.ToString());
        }

        [Test]
        public void CanFetchManyThenTwoColumns()
        {
            var spec = Category.Do.FetchMany(x => x.Products).ThenFetch(x => x.Name);
            var queryable = new EmptyQueryable<Category>("q").ApplySpecs(spec);
            Assert.AreEqual("q.FetchMany(x => x.Products).ThenFetch(x => x.Name)",
                queryable.Expression.ToString());
        }

        [Test]
        public void CanSerializeFetchAllNormal()
        {
            var spec = Customer.Do.Fetch(x => x.Address).Fetch(x => x.ContactName);
            var spec2 = SimpleSerializer.Binary().RoundTrip(spec);

            Assert.AreNotSame(spec, spec2);
        }

        [Test]
        public void CanSerializeFetchWithThen()
        {
            var spec = Product.Do.Fetch(x => x.Category).ThenFetch(x => x.Name);
            var spec2 = SimpleSerializer.Binary().RoundTrip(spec);

            Assert.AreNotSame(spec, spec2);
        }

        [Test]
        public void CanSerializeComplexOne()
        {
            var spec = Category.Do.FetchMany(x => x.Products).ThenFetch(x => x.Category).ThenFetchMany(x => x.Products).ThenFetch(x => x.Name);
            var spec2 = SimpleSerializer.Binary().RoundTrip(spec);

            Assert.AreNotSame(spec, spec2);
        }
    }
}
