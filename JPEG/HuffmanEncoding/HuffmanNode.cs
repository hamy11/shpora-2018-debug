using System;
using System.Collections.Generic;

namespace JPEG.HuffmanEncoding
{
    internal class HuffmanNode : IComparable<HuffmanNode>
    {
        public readonly byte? LeafLabel;
        public readonly int Frequency;
        public readonly HuffmanNode Left;
        public readonly HuffmanNode Right;

        public HuffmanNode(HuffmanNode left, HuffmanNode right, int frequency, byte? leafLabel)
        {
            Left = left;
            Right = right;
            Frequency = frequency;
            LeafLabel = leafLabel;
        }

        public override bool Equals(object obj)
        {
            return obj is HuffmanNode node &&
                   EqualityComparer<byte?>.Default.Equals(LeafLabel, node.LeafLabel) &&
                   Frequency == node.Frequency &&
                   EqualityComparer<HuffmanNode>.Default.Equals(Left, node.Left) &&
                   EqualityComparer<HuffmanNode>.Default.Equals(Right, node.Right);
        }

        public override int GetHashCode()
        {
            var hashCode = -456731829;
            hashCode = hashCode * -1521134295 + EqualityComparer<byte?>.Default.GetHashCode(LeafLabel);
            hashCode = hashCode * -1521134295 + Frequency.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<HuffmanNode>.Default.GetHashCode(Left);
            hashCode = hashCode * -1521134295 + EqualityComparer<HuffmanNode>.Default.GetHashCode(Right);
            return hashCode;
        }

        public override string ToString()
        {
            return Frequency.ToString() + Left + Right;
        }

        public int CompareTo(HuffmanNode other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            return other.Frequency < Frequency ? 1 : -1;
        }
    }
}