using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Expression;

namespace Arc.CompilerCommandGenerator
{
    internal class Utils
    {
        internal static byte[] CombineLeadingCommand(params byte[] commands)
        {
            var result = new List<byte>();

            bool flip = false;
            foreach (var b in commands)
            {
                if (flip)
                {
                    result[^1] *= 0x10;
                    result[^1] += b;
                }
                else
                {
                    result.Add(b);
                }

                flip = !flip;
            }

            return result.ToArray();
        }

        internal static SimpleExpression ExpressionInfixToPostfix(SimpleExpression expression)
        {
            var operatorStack = new Stack<ExpressionTerm>();
            var result = new List<ExpressionTerm>();

            foreach (var term in expression.Terms)
            {
                switch (term.TermType)
                {
                    case ExpressionTermType.Data:
                        {
                            // Push all terms into result directly (infix to postfix)
                            result.Add(term);
                            break;
                        }
                    case ExpressionTermType.Operator:
                        {
                            // The previous TermType::Priority must increased the priority level
                            while (operatorStack.TryPeek(out var r) && r.TermType == ExpressionTermType.Operator)
                            {
                                // Pop if operator priority is higher than current operator
                                if (TokenConstants.CalculationOperatorPriority[r.GetOperator()!.CalculationOperator] > TokenConstants.CalculationOperatorPriority[term.GetOperator()!.CalculationOperator])
                                {
                                    result.Add(operatorStack.Pop());
                                }
                                else
                                {
                                    break;
                                }
                            }

                            operatorStack.Push(term);

                            break;
                        }
                    case ExpressionTermType.OpenPriority:
                        {
                            operatorStack.Push(term);
                            break;
                        }
                    case ExpressionTermType.ClosePriority:
                        {
                            // Pop until bracket
                            while (operatorStack.TryPeek(out var r) && r.TermType == ExpressionTermType.Operator)
                            {
                                result.Add(operatorStack.Pop());

                            }

                            // Pop this bracket (it won't be transferred to result)
                            operatorStack.Pop();

                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException("Invalid ExpressionTermType");
                        }
                }
            }

            // Push all operators remained
            result.AddRange(operatorStack.ToArray());

            return new(result.ToArray(), expression.OutputDataType);
        }

        internal static byte[] BuildDataBlock(byte[] encodedData, PackageMetadata metadata)
        {
            var result = new List<byte>();

            var data = encodedData.ToList();
            if (data.Count % metadata.DataSectionSize > 0)
            {
                data.InsertRange(0, new byte[data.Count % metadata.DataSectionSize]);
            }

            result.Add((byte)(data.Count / metadata.DataSectionSize));
            result.AddRange(data);

            return result.ToArray();
        }

        internal static byte[] GenerateSlotData(long slot, PackageMetadata metadata)
        {
            var slotBytes = BitConverter.GetBytes(slot).ToArray();
            Array.Resize(ref slotBytes, metadata.DataSlotAlignment);
            Array.Reverse(slotBytes);

            return slotBytes;
        }
    }
}