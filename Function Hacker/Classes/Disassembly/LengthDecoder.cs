using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FunctionHacker.Classes.Disassembly
{
    public struct SIMPLE_INSTRUCTION
    {
        public const ushort OP_RET_NULL = 0xC3;
        public const ushort OP_RET_IMM16 = 0xC2;
        public const ushort OP_CALL_IMM32 = 0xE8;
        public const ushort OP_JMP_IMM32 = 0xE9;
        public const ushort OP_MOV_REG32 = 0x8B;
        public const ushort OP_PUSH_DEREF_REG32 = 0xFF;

        public uint address;
        public ushort opcode;
        public ushort size;
        public uint immediate;
        public uint jmp_target;
        public uint numPrefix;

        

        public SIMPLE_INSTRUCTION(uint address, ushort opcode, ushort size, uint immediate, uint jmp_target, uint numPrefix)
        {
            this.address = address;
            this.opcode = opcode;
            this.size = size;
            this.immediate = immediate;
            this.jmp_target = jmp_target;
            this.numPrefix = numPrefix;
        }
    }

    public class LengthDecoder
    {
        private static Hashtable warningOpCodeTable = new Hashtable()
	    {
            // {opcode, level of warning}
		    {0x0,100},
            {0x16,1},
            {0x27,1},
            {0x2F,1},
            {0x3F,1},
            {0x9F,1},
            {0xDC,1},
            {0xDE,1},
            {0xEF,1},
            {0xF1,1},
            {0xF4,1},
            {0xFA,1},
            {0xFB,1},
            {0xFC,1}
	    };

        /// <summary>
        /// Add first RET in the data block to the return address list.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="baseAddress"></param>
        /// <param name="returnAddresses"></param>
        /// <returns></returns>
        public static void AddFirstRet(byte[] data, int baseAddress, int startOffset, ref oAsmRetList returnAddresses)
        {
            // Disassemble all the instructions in the data block
            int offset = startOffset;
            while (offset + 10 < data.Length)
            {
                // Process this instruction

                // Decode
                SIMPLE_INSTRUCTION instruction = decodeInstruction(data, offset, baseAddress);

                // Process
                switch (instruction.opcode)
                {
                    case SIMPLE_INSTRUCTION.OP_RET_NULL:
                        if (instruction.size == 1)
                            returnAddresses.addRet(instruction.address, 0);
                        return;
                    case SIMPLE_INSTRUCTION.OP_RET_IMM16:
                        if (instruction.size == 3)
                            returnAddresses.addRet(instruction.address, instruction.immediate);
                        return;
                }

                // Next instruction
                offset += instruction.size;
            }
        }


        

        /// <summary>
        /// Return instructions are outputted to returnAddresses.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="baseAddress"></param>
        /// <param name="returnAddresses"></param>
        /// <returns></returns>
        public static void DisassembleBlockRetOnly(byte[] data, int baseAddress, ref oAsmRetList returnAddresses)
        {
            // Disassemble all the instructions in the data block
            int offset = 0;
            while (offset + 10 < data.Length)
            {
                // Process this instruction

                // Decode
                SIMPLE_INSTRUCTION instruction = decodeInstruction(data, offset, baseAddress);

                // Process
                switch (instruction.opcode)
                {
                    case SIMPLE_INSTRUCTION.OP_RET_NULL:
                        if( instruction.size == 1 )
                            returnAddresses.addRet(instruction.address, 0);
                        break;
                    case SIMPLE_INSTRUCTION.OP_RET_IMM16:
                        if (instruction.size == 3)
                            returnAddresses.addRet(instruction.address, instruction.immediate);
                        break;
                }

                // Next instruction
                offset += instruction.size;
            }
        }


        /// <summary>
        /// Counts the number of warnings for the data block. If escapeOnRet is set, any
        /// instrutions after a return will be ignored.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="baseAddress"></param>
        /// <param name="escapeOnRet"></param>
        /// <param name="instructionAddress"></param>
        /// <returns></returns>
        public static int CountWarnings(byte[] data)
        {
            // Disassemble all the instructions in the data block
            int offset = 0;
            int warnings = 0;
            while (offset + 6 < data.Length)
            {
                // Process this instruction

                // Decode
                SIMPLE_INSTRUCTION instruction = decodeInstruction(data, offset, 0);
                
                // Check the warning cost of this opcode
                if( warningOpCodeTable.ContainsKey((int)instruction.opcode) )
                {
                    // There exists a warning for this opcode
                    warnings += (int)warningOpCodeTable[(int)instruction.opcode];
                }

                // Next instruction
                offset += instruction.size;
            }
            return warnings;
        }

        /// <summary>
        /// This processes instructions starting at the beginning of the memory block, and searches for the
        /// the smallest number of instructions that form at least 5 bytes.
        /// </summary>
        /// <param name="readMemory"></param>
        /// <returns></returns>
        public static byte[] getMinFiveBytesCode(byte[] data)
        {
            int offset = 0;
            while (offset + 5 < data.Length && offset < 5)
            {
                // Decode this instruction
                SIMPLE_INSTRUCTION instruction = decodeInstruction(data, offset, 0);

                // Next instruction
                offset += instruction.size;
            }

            // Generate the result
            byte[] result = new byte[offset];
            Array.ConstrainedCopy(data,0,result,0,offset);

            return result;
        }


        /// <summary>
        /// Decodes all the call instructions from the block. Return instructions are outputted to returnAddresses.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="baseAddress"></param>
        /// <param name="returnAddresses"></param>
        /// <returns></returns>
        public static List<SIMPLE_INSTRUCTION> DisassembleBlockCallsOnly(byte[] data, int baseAddress, ref oAsmRetList returnAddresses, ref oEbpArgumentList ebpArguments, ref List<jmpInstruction> jumps)
        {
            // Disassemble all the instructions in the data block
            int offset = 0;
            List<SIMPLE_INSTRUCTION> result = new List<SIMPLE_INSTRUCTION>(data.Length/100);
            while( offset + 10 < data.Length)
            {
                // Process this instruction

                // Decode
                SIMPLE_INSTRUCTION instruction = decodeInstruction(data, offset, baseAddress);
                
                // Process
                switch(instruction.opcode)
                {
                    case SIMPLE_INSTRUCTION.OP_CALL_IMM32:
                        if( instruction.size == 5 && instruction.numPrefix == 0 )
                            result.Add(instruction);
                        break;
                    case SIMPLE_INSTRUCTION.OP_RET_NULL:
                        if (instruction.size == 1 && instruction.numPrefix == 0)
                            returnAddresses.addRet(instruction.address, 0);
                        break;
                    case SIMPLE_INSTRUCTION.OP_RET_IMM16:
                        if (instruction.size == 3 && instruction.numPrefix == 0)
                            returnAddresses.addRet(instruction.address, instruction.immediate);
                        break;
                    case SIMPLE_INSTRUCTION.OP_MOV_REG32:
                        if (instruction.size == 3 && (data[offset+1] == 0x45 || data[offset+1] == 0x5d ||
                                                      data[offset+1] == 0x4D || data[offset+1] == 0x55 ||
                                                      data[offset+1] == 0x75 || data[offset+1] == 0x7D) &&
                            data[offset + 2] <= 0x7F && data[offset + 2] > 0x04)
                        {
                            // mov reg32, [ebp+imm8]
                            // Save this immediate value, it tells us how many arguments this function has
                            ebpArguments.addEbpReference(instruction.address, (int) (data[offset + 2]-4)/4);
                        }
                        break;
                    case SIMPLE_INSTRUCTION.OP_PUSH_DEREF_REG32:
                        if (instruction.size == 3 && data[offset + 1] == 0x75 &&
                            data[offset + 2] <= 0x7F && data[offset + 2] > 0x04)
                        {
                            // push [ebp+imm8]
                            // Save this immediate value, it tells us how many arguments this function has
                            ebpArguments.addEbpReference(instruction.address, (int)(data[offset + 2] - 4) / 4);
                        }
                        break;
                    case SIMPLE_INSTRUCTION.OP_JMP_IMM32:
                        if (instruction.size == 5)
                        {
                            // jmp imm32
                            // Save this jump
                            jumps.Add( new jmpInstruction(instruction.address, instruction.jmp_target) );
                        }
                        break;

                }

                // Check to see if it is a mov reg32, [ebp+imm8] instruction.

                // Next instruction
                offset += instruction.size;
            }
            
            // Return the resulting call instructions
            return result;
        }

        /// <summary>
        /// Decodes the data into an array of SIMPLE_INSTRUCTIONs,
        /// </summary>
        /// <param name="data">Byte array of data.</param>
        /// <param name="baseAddress">The address of the first byte of the data.</param>
        /// <returns>Decoded instructions</returns>
        public static List<SIMPLE_INSTRUCTION> DisassembleBlock(byte[] data, int baseAddress)
        {
            // Disassemble all the instructions in the data block
            int offset = 0;
            List<SIMPLE_INSTRUCTION> result = new List<SIMPLE_INSTRUCTION>(data.Length / 3);
            while (offset + 10 < data.Length)
            {
                // Process this instruction
                result.Add( decodeInstruction(data,offset, baseAddress) );
                offset += result[result.Count - 1].size;
            }
            
            // Return the resulting instructions
            return result;
        }
        // opcode 00
        // 

        private static SIMPLE_INSTRUCTION decodeInstruction(byte[] data, int offset, int baseAddress) 
        {
            // Most of this code is couresty Nicolas 'Nick' Capens: http://www.devmaster.net/codespotlight/show.php?id=25

            int startOffset = offset;

            // Skip prefixes F0h, F2h, F3h, 66h, 67h, D8h-DFh, 2Eh, 36h, 3Eh, 26h, 64h and 65h
            int operandSize = 4; 
            int FPU = 0;
            int numPrefix = 0;
            while(data[offset] == 0xF0 || 
                  data[offset] == 0xF2 || 
                  data[offset] == 0xF3 || 
                 (data[offset] & 0xFC) == 0x64 || 
                 (data[offset] & 0xF8) == 0xD8 ||
                 (data[offset] & 0x7E) == 0x62)
            { 
                if(data[offset] == 0x66) 
                { 
                    operandSize = 2; 
                }
                else if((data[offset] & 0xF8) == 0xD8) 
                {
                    FPU = data[offset++];
                    break;
                }
                numPrefix++;
                offset++;
            }

            // Skip two-byte opcode byte 
            bool twoByte = false; 
            if(data[offset] == 0x0F) 
            { 
                twoByte = true; 
                offset++; 
            } 

            // Skip opcode byte 
            byte opcode = data[offset++]; 

            // Skip mod R/M byte 
            byte modRM = 0xFF; 
            if(FPU != 0) 
            { 
                if((opcode & 0xC0) != 0xC0) 
                { 
                    modRM = opcode; 
                } 
            } 
            else if(!twoByte) 
            { 
                if((opcode & 0xC4) == 0x00 || 
                   (opcode & 0xF4) == 0x60 && ((opcode & 0x0A) == 0x02 || (opcode & 0x09) == 0x9) || 
                   (opcode & 0xF0) == 0x80 || 
                   (opcode & 0xF8) == 0xC0 && (opcode & 0x0E) != 0x02 || 
                   (opcode & 0xFC) == 0xD0 || 
                   (opcode & 0xF6) == 0xF6) 
                {
                    modRM = data[offset++]; 
                } 
            } 
            else 
            { 
                if((opcode & 0xF0) == 0x00 && (opcode & 0x0F) >= 0x04 && (opcode & 0x0D) != 0x0D || 
                   (opcode & 0xF0) == 0x30 || 
                   opcode == 0x77 || 
                   (opcode & 0xF0) == 0x80 || 
                   (opcode & 0xF0) == 0xA0 && (opcode & 0x07) <= 0x02 || 
                   (opcode & 0xF8) == 0xC8) 
                { 
                    // No mod R/M byte 
                } 
                else 
                {
                    modRM = data[offset++]; 
                } 
            } 

            // Skip SIB
            if((modRM & 0x07) == 0x04 &&
               (modRM & 0xC0) != 0xC0)
            {
                offset++;   // SIB
            }

            // Skip displacement
            if ((modRM & 0xC5) == 0x05) offset += 4;   // Dword displacement, no base 
            if ((modRM & 0xC0) == 0x40) offset += 1;   // Byte displacement 
            if ((modRM & 0xC0) == 0x80) offset += 4;   // Dword displacement 

            // Skip immediate 
            if(FPU != 0) 
            { 
                // Can't have immediate operand 
            } 
            else if(!twoByte) 
            { 
                if((opcode & 0xC7) == 0x04 || 
                   (opcode & 0xFE) == 0x6A ||   // PUSH/POP/IMUL 
                   (opcode & 0xF0) == 0x70 ||   // Jcc 
                   opcode == 0x80 || 
                   opcode == 0x83 || 
                   (opcode & 0xFD) == 0xA0 ||   // MOV 
                   opcode == 0xA8 ||            // TEST 
                   (opcode & 0xF8) == 0xB0 ||   // MOV
                   (opcode & 0xFE) == 0xC0 ||   // RCL 
                   opcode == 0xC6 ||            // MOV 
                   opcode == 0xCD ||            // INT 
                   (opcode & 0xFE) == 0xD4 ||   // AAD/AAM 
                   (opcode & 0xF8) == 0xE0 ||   // LOOP/JCXZ 
                   opcode == 0xEB || 
                   opcode == 0xF6 && (modRM & 0x30) == 0x00)   // TEST 
                { 
                    offset += 1; 
                } 
                else if((opcode & 0xF7) == 0xC2) 
                {
                    offset += 2;   // RET 
                } 
                else if((opcode & 0xFC) == 0x80 || 
                        (opcode & 0xC7) == 0x05 || 
                        (opcode & 0xF8) == 0xB8 ||
                        (opcode & 0xFE) == 0xE8 ||      // CALL/Jcc 
                        (opcode & 0xFE) == 0x68 || 
                        (opcode & 0xFC) == 0xA0 || 
                        (opcode & 0xEE) == 0xA8 || 
                        opcode == 0xC7 || 
                        opcode == 0xF7 && (modRM & 0x30) == 0x00) 
                {
                    offset += operandSize; 
                } 
            } 
            else 
            { 
                if(opcode == 0xBA ||            // BT 
                   opcode == 0x0F ||            // 3DNow! 
                   (opcode & 0xFC) == 0x70 ||   // PSLLW 
                   (opcode & 0xF7) == 0xA4 ||   // SHLD 
                   opcode == 0xC2 || 
                   opcode == 0xC4 || 
                   opcode == 0xC5 || 
                   opcode == 0xC6) 
                {
                    offset += 1; 
                } 
                else if((opcode & 0xF0) == 0x80) 
                {
                    offset += operandSize;   // Jcc -i
                }
            }

            // Return the details on this instruction
            ushort size = (ushort) (offset - startOffset);
            uint address = (uint) (baseAddress + startOffset);
            ushort fullOpcode = (ushort) ((twoByte ? (0x0F << 8) + opcode : opcode));

            uint immediate = 0;
            switch(size - numPrefix - (twoByte ? 2 : 1))
            {
                case 1:
                    immediate = (uint)data[(int)(startOffset + numPrefix + (twoByte ? 2 : 1))];
                    break;
                case 2:
                    immediate = oMemoryFunctions.ByteArrayToUshort(data, (int)(startOffset + numPrefix + (twoByte ? 2 : 1)));
                    break;
                case 4:
                    immediate = oMemoryFunctions.ByteArrayToUint(data, (int)(startOffset + numPrefix + (twoByte ? 2 : 1)));
                    break;
                default:
                    break;
            }

            // Resolve the jump target if it is a call or jump instruction
            uint jmpTarget = 0;
            if (fullOpcode == SIMPLE_INSTRUCTION.OP_CALL_IMM32 || fullOpcode == SIMPLE_INSTRUCTION.OP_JMP_IMM32)
            {
                // Resolve the jump target
                jmpTarget = (address + 5) + immediate;
            }

            return new SIMPLE_INSTRUCTION(address, fullOpcode, size, immediate, jmpTarget, (uint) numPrefix);
        }

        
    }
}
