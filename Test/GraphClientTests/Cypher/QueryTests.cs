﻿using System;
using NUnit.Framework;

namespace Neo4jClient.Test.GraphClientTests.Cypher
{
    [TestFixture]
    public class QueryTests
    {
        readonly Uri fakeEndpoint = new Uri("http://test.example.com/foo");

        [Test]
        public void StartAndReturnNodeById()
        {
            // http://docs.neo4j.org/chunked/1.6/query-start.html#start-node-by-id
            // START n=node(1)
            // RETURN n

            var client = new GraphClient(fakeEndpoint);
            var query = client
                .Cypher
                .Start("n", (NodeReference) 1)
                .Return("n")
                .Query;

            Assert.AreEqual("START n=node({p0})\r\nRETURN n", query.QueryText);
            Assert.AreEqual(1, query.QueryParameters["p0"]);
        }

        [Test]
        public void MultipleStartPoints()
        {
            // http://docs.neo4j.org/chunked/1.6/query-start.html#start-multiple-start-points
            // START a=node(1), b=node(2)
            // RETURN a,b

            var client = new GraphClient(fakeEndpoint);
            var query = client
                .Cypher
                .Start("a", (NodeReference)1)
                .AddStartPoint("b", (NodeReference)2)
                .Return("a", "b")
                .Query;

            Assert.AreEqual("START a=node({p0}), b=node({p1})\r\nRETURN a, b", query.QueryText);
            Assert.AreEqual(1, query.QueryParameters["p0"]);
            Assert.AreEqual(2, query.QueryParameters["p1"]);
        }

        [Test]
        public void ReturnFirstPart()
        {
            // http://docs.neo4j.org/chunked/1.6/query-limit.html#limit-return-first-part
            // START n=node(3, 4, 5, 1, 2)
            // RETURN n
            // LIMIT 3

            var client = new GraphClient(fakeEndpoint);
            var query = client
                .Cypher
                .Start("n", (NodeReference)3, (NodeReference)4, (NodeReference)5, (NodeReference)1, (NodeReference)2)
                .Return("n")
                .Limit(3)
                .Query;

            Assert.AreEqual("START n=node({p0}, {p1}, {p2}, {p3}, {p4})\r\nRETURN n\r\nLIMIT {p5}", query.QueryText);
            Assert.AreEqual(3, query.QueryParameters["p0"]);
            Assert.AreEqual(4, query.QueryParameters["p1"]);
            Assert.AreEqual(5, query.QueryParameters["p2"]);
            Assert.AreEqual(1, query.QueryParameters["p3"]);
            Assert.AreEqual(2, query.QueryParameters["p4"]);
            Assert.AreEqual(3, query.QueryParameters["p5"]);
        }

        [Test]
        public void MatchRelatedNodes()
        {
            // http://docs.neo4j.org/chunked/1.6/query-match.html#match-related-nodes
            // START n=node(3)
            // MATCH (n)--(x)
            // RETURN x

            var client = new GraphClient(fakeEndpoint);
            var query = client
                .Cypher
                .Start("n", (NodeReference)3)
                .Match("(n)--(x)")
                .Return("x")
                .Query;

            Assert.AreEqual("START n=node({p0})\r\nMATCH (n)--(x)\r\nRETURN x", query.QueryText);
            Assert.AreEqual(3, query.QueryParameters["p0"]);
        }
    }
}
