using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FluentApi.Graph
{
    public enum NodeShape
    {
        Box,
        Ellipse
    }

    public class DotGraphBuilder
    {
        public static GraphBuilder DirectedGraph(string graphName)
        {
            return new GraphBuilder(graphName, true, true);
        }

        public static GraphBuilder UndirectedGraph(string graphName)
        {
            return new GraphBuilder(graphName, false, true);
        }
    }

    public class GraphBuilder
    {
        internal Graph Graph { get; }

        public GraphBuilder(string graphName, bool directed, bool strict)
        {
            Graph = new Graph(graphName, directed, strict);
        }

        internal GraphBuilder(GraphBuilder graphBuilder)
        {
            Graph = graphBuilder.Graph;
        }

        public GraphBuilderWithNodeAttributesBuilder AddNode(string name)
        {
            Graph.AddNode(name);
            return new GraphBuilderWithNodeAttributesBuilder(this);
        }

        public GraphBuilderWithEdgeAttributesBuilder AddEdge(string sourceNode, string destinationNode)
        {
            Graph.AddEdge(sourceNode, destinationNode);
            return new GraphBuilderWithEdgeAttributesBuilder(this);
        }

        public string Build() => Graph.ToDotFormat();
    }

    public class GraphBuilderWithNodeAttributesBuilder : GraphBuilder
    {
        public GraphBuilderWithNodeAttributesBuilder(GraphBuilder graphBuilder)
            : base(graphBuilder) { }

        public GraphBuilder With(Action<NodeAttributesBuilder> attributesSetter)
        {
            attributesSetter(new NodeAttributesBuilder(Graph.Nodes.Last().Attributes));
            return this;
        }
    }

    public class GraphBuilderWithEdgeAttributesBuilder : GraphBuilder
    {
        public GraphBuilderWithEdgeAttributesBuilder(GraphBuilder graphBuilder)
            : base(graphBuilder) { }

        public GraphBuilder With(Action<EdgeAttributesBuilder> attributesSetter)
        {
            attributesSetter(new EdgeAttributesBuilder(Graph.Edges.Last().Attributes));
            return this;
        }
    }

    public abstract class DefaultAttributesBuilder<T>
    {
        private Dictionary<string, string> Attributes { get; }
        private T CurrentAttributesBuilder { get; set; }

        internal DefaultAttributesBuilder(Dictionary<string, string> attributes)
        {
            Attributes = attributes;
        }

        internal void SetCurrentAttributesBuilder(T attributesBuilder)
        {
            CurrentAttributesBuilder = attributesBuilder;
        }

        internal T AddAttribute(string name, string value)
        {
            Attributes[name] = value;
            return CurrentAttributesBuilder;
        }

        public T Color(string value) => AddAttribute("color", value);

        public T FontSize(int value) => AddAttribute("fontsize", value.ToString());

        public T Label(string value) => AddAttribute("label", value);
    }

    public class NodeAttributesBuilder : DefaultAttributesBuilder<NodeAttributesBuilder>
    {
        public NodeAttributesBuilder(Dictionary<string, string> attributes)
            : base(attributes)
        {
            SetCurrentAttributesBuilder(this);
        }

        public NodeAttributesBuilder Shape(NodeShape value) => AddAttribute("shape", value.ToString().ToLower());
    }

    public class EdgeAttributesBuilder : DefaultAttributesBuilder<EdgeAttributesBuilder>
    {
        public EdgeAttributesBuilder(Dictionary<string, string> attributes) 
            : base(attributes)
        {
            SetCurrentAttributesBuilder(this);
        }

        public EdgeAttributesBuilder Weight(double value) 
            => AddAttribute("weight", value.ToString(CultureInfo.InvariantCulture));
    }
}