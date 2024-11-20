# Prefix

前缀在原始方法之前执行：

* 访问和编辑原始方法的参数
* 设置原始方法的结果
* 跳过原始方法
* 定义在Postfix中会使用到的state

## 修改参数

```csharp
public class OriginalCode{
    public void Test(int counter, string name)
    {
        // ...
    }
}

[HarmonyPatch(typeof(OriginalCode), nameof(OriginalCode.Test))]
class Patch{
    public static void Prefix(int counter, ref string name){
        // 获取参数
        FileLog.Log("counter = " + counter);
       // 改值
        name = "test";
    }
}
```

## 修改结果或跳过原始方法的执行

修改原始结果需要参数`__result`，类型必须与返回值类型相同

如果要跳过原始方法的执行需要让patch返回一个bool类型的值，false为跳过，true为继续执行

```csharp
public class OriginalCode{
    String name = "original";
    public string GetName() => name; // ...
}

[HarmonyPatch(typeof(OriginalCode), nameof(OriginalCode.GetName))]
class Patch{
    static bool Prefix(ref string __result){
        // 修改结果
        __result = "test";
        // 不跳过原始方法，结果为original
        return true;
        // 跳过原始方法，结果为test
        return false;
    }
}
```

# Postfix

在原始方法之后执行，通常用于：

* 获取或更改原始方法的结果
* 访问原始方法的参数
* 获取Prefix传递的状态


## 获取或更改原始方法的结果

修改原始结果需要参数`__result`，类型必须与返回值类型相同

```csharp
public class OriginalCode {
    private string name;
    public  string GetName() => name; // ...
}

[HarmonyPatch(typeof(OriginalCode), nameof(OriginalCode.GetName))]
class Patch {
    static void Postfix(ref string __result) {
        if (__result == "foo") __result = "bar";
    }
}
```

## 获取原始方法的参数

```csharp
public class OriginalCode {
    public void Test(int counter) {
        // ...
    }
}

[HarmonyPatch(typeof(OriginalCode), nameof(OriginalCode.Test))]
class Patch {
    static void Prefix(int counter) => FileLog.Log("counter = " + counter);
}
```


# Transpiler

Tanspiler并不在运行时(runtime)调用，而是在编译时进行更改（详见.net执行原理）

ps：因为是编译器执行，因此无法获取任何运行时状态（例如参数值和返回结果值）

一个例子

```csharp
// 获取类中的字段信息
static FieldInfo exam1 = AccessTools.Field(typeof(类名exam_class1), nameof(类名.字段名exam_field1));

// 获取类中的方法信息
static MethodInfo exam2 = SymbolExtensions.GetMethodInfo(() => 类名exam_class2.方法名()exam_method2);

// IL 操作器方法，用于在 exam_field1 赋值之前插入对 exam_method2 的调用
static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions){
	// 标记是否找到了目标字段的赋值指令
    bool found = false;

    // 遍历传入的 IL 指令集合
    foreach (var instruction in instructions){
        // 检查当前指令是否是将值存储到 exam1 字段的指令
        if (instruction.StoresField(exam1)){
            // 插入一个新的 CALL 指令，调用 exam2 方法
            yield return new CodeInstruction(OpCodes.Call, exam2);
            found = true; // 标记已经找到了目标字段的赋值指令
        }

        // 将当前指令原样返回
        yield return instruction;
    }

    // 如果遍历完所有指令后仍未找到目标字段的赋值指令，则报告错误
    if (!found) ReportError("Cannot find <Stdfld someField> in OriginalType.OriginalMethod");
}
```

PS：因为transpiler过于复杂，需要对C#具体执行原理有一定了解，因此只给简单说明

## 一个例子
ps：IL由操作数+操作码组成，更详细的详见计算机组成原理

假设IL代码长这样
```il
IL_0078: ldarg.0
IL_0079: ldfld bool RimWorld.Dialog_FormCaravan::reform
IL_007e: brtrue IL_00ac
IL_0083: ldarg.0
IL_0084: call instance float32 RimWorld.Dialog_FormCaravan::get_MassUsage()
IL_0089: ldarg.0
IL_008a: call instance float32 RimWorld.Dialog_FormCaravan::get_MassCapacity()
IL_008f: ble.un IL_00ac
IL_0094: ldarg.0
IL_0095: call instance void RimWorld.Dialog_FormCaravan::FlashMass()
IL_009a: ldstr "TooBigCaravanMassUsage"
IL_009f: call string Verse.Translator::Translate(string)
IL_00a4: ldc.i4.2
IL_00a5: call void Verse.Messages::Message(string, valuetype Verse.MessageSound)
IL_00aa: ldc.i4.0
IL_00ab: ret
```

对应的C#代码长这样

```csharp
if (!this.reform && this.MassUsage > this.MassCapacity){
	this.FlashMass();
	Messages.Message("TooBigCaravanMassUsage".Translate(), MessageSound.RejectInput);
	return false;
}
```

关于这段代码：首先是`this.reform`，这个操作是获取`this`的`reform`字段，CPU首先会让`this`入栈(即栈顶是`this`)，之后扫描到`.reform`的时候会将`this`出栈，然后提取出`this`的`reform`字段，最后将字段的值入栈，即栈顶是`reform`

对于`this.reform`之前的`!`操作，编译器判定`this.reform`为`true`时会跳过代码（`IL`中的`IL_00ac`），判断`this.reform`是否为`true`同样需要出栈，因此执行完`!this.reform`后栈为空

之后执行`this.MassUsage > this.MassCapacity`，执行过程与前面的相同
1. `this`入栈
2. 到了`.MassUsage`后`this`出栈，`MassUsage`入栈
3. `this`入栈
4. `this`出栈，`MassCapacity`入栈
5. `MassUsage`和`MassCapacity`出栈，比较，需要注意的是这里的比较用的是`<=`不是代码中的`>`(`IL_008f: ble.un IL_00ac`)

下面是方法的执行

先回到IL代码的开头`IL_0078: ldarg.0`这段话的意思是，将第0个参数加在到栈上，第0个参数是`this`（ps：所有类的非静态方法的第一个参数都是`this`，就像py的`self`一样，只是大部分语言隐藏了）

这里`this`的类型是`Dialog_FormCaravan`类型，因此执行第一行之后栈顶元素是`Dialog_FormCarvan`类型的实例对象，因此调用一个实例方法必须先将该方法的对象入栈，之后将方法的所有参数依次入栈，最后调用`CALL`执行方法

例如C#代码中的`this.MassUsage`对应的就是（ps：`this.MassUsage`是`this.getMassUsage()`的缩写，面向对象一切都是对象方法的调用）

**PS2：ld是链接，arg是参数，ldarg就是加载参数**

```il
IL_0083: ldarg.0
IL_0084: call instance float32 RimWorld.Dialog_FormCaravan::get_MassUsage()
```

**PS3：使用IL可以实现任何功能，因此有些无法用Profix和Postfix实现的可以用IL**
**PS4：栈上的每一个数字都有用，因此不能像写高级代码一样在栈上写冗余的数字**

接下来的目的是从源代码中删除上面那个代码片段，对IL代码简化一下，删除所有不会改变指令流程的代码，之后代码就变成了

```il
IL_0077: ret

IL_0078: ...codes...
IL_007e: brtrue IL_00ac
IL_0083: ...codes...
IL_008f: ble.un IL_00ac
IL_0094: ...codes...
IL_00ab: ret

IL_00ac: ...codes...
```

要删除有几个方案：
1. 将78-ab的所有代码都替换为NOPs
2. 在77之后插入一条无条件转移指令，直接跳转到ac
3. 根据这段代码特点，还有其他方式

搜索 `ret` 代码。对于找到的每个 `ret` 代码，继续搜索直到下一个 `ret`，并查找字符串 `TooBigCaravanMassUsage` 。如果找到该字符串，删除从第一个 `ret` 之后到包括第二个 `ret` 的所有内容并继续查找下一个 `ret`

```csharp
[HarmonyPatch(typeof(Dialog_FormCaravan), nameof(Dialog_FormCaravan.CheckForErrors))]
// 例子的目的是为了移除上面那段代码
public static class Dialog_FormCaravan_CheckForErrors_Patch {
    // 方法名是 Transpiler，表示这是一个 Transpiler，而不是一个 Patch
    // 方法签名必须返回 IEnumerable<CodeInstruction> 这是IL指令集合
    // 参数中必须包含 IEnumerable<CodeInstruction> instructions
    // 还可以通过 ILGenerator generator 注入代码生成器，或通过 MethodBase method 注入原始方法的信息
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        // 用于标记是否找到了 TooBigCaravanMassUsage
        var foundMassUsageMethod = false;
        // 用于记录需要移除的代码的起始和结束索引
        var startIndex           = -1;
        var endIndex             = -1;

        // 通过传入的 IL 获取 ILcodes
        var codes = new List<CodeInstruction>(instructions);
        for (var i = 0; i < codes.Count; i++) {
            // 遍历 ILcodes，并查找 ret
            if (codes[i].opcode == OpCodes.Ret) {
                // 如果找到了字符串，则 endIndex更改为当前位置
                if (foundMassUsageMethod) {
                    endIndex = i;
                    break;
                } else {
                    // 如果没有找到，将startIndex设置为 ret 的下一个位置
                    startIndex = i + 1;

                    // 然后从 startIndex 开始继续查找，直到找到下一个 ret
                    for (var j = startIndex; j < codes.Count; j++) {
                        if (codes[j].opcode == OpCodes.Ret) break;

                        var strOperand = codes[j].operand as string;
                        // 判定有没有要找的字符串
                        if (strOperand == "TooBigCaravanMassUsage") {
                            foundMassUsageMethod = true;
                            break;
                        }
                    }
                }
            }
        }

        // 移除代码
        if (startIndex > -1 && endIndex > -1) {
            // 要删除代码不能直接删除，删除某行会让IL的编号混乱，其他跳转会混乱，因此用的是 Nop 空操作
            codes[startIndex].opcode = OpCodes.Nop;
            codes.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
        }

        // 返回修改后的指令集
        return codes.AsEnumerable();
    }
}
```
## codeInstruction

`Transpiler` 的核心是`codeInstruction`，`codeInstruction`是对`.NET Emit`命名空间的抽象

每个 `transpiler` 都需要传入 `CodeInstruction` 列表，并返回修改后的 `CodeInstruction` 列表，还可以在 `transpiler` 中注入 `ILGenerator` 创建一个或多个 `Label` 和表示局部变量的 `LocalBuilder` 实例

关于`Emit`：`Emit`接受一个`OpCode`，操作数：
* 类型Type
* 字段FieldInfo
* 方法MethodInfo
* 构造函数ConstructorInfo
* 基本数据类型：int64\32\16...

特殊的：
* 跳转到操作数必须是`Label`不能是具体数值
* 有局部变量时避免使用索引
* 操作后会自动调用`Emit()`，因此代码中不需要调用

关于标签`Label`：所有原始标签都由 `Label` 对象表示，用于 `CodeInstruction` 的操作数或它的 `labels` 字段（类型为 `Label[]`），跳转时需要指定跳转的 `Opcode` 和标签作为操作数。然后将标签附加到目标的标签。要创建一个新的跳转标签，使用 `ILGenerator.DefineLabel()` 并将该标签放入目标 `CodeInstruction` 的 `labels` 字段。使用该标签作为跳转操作码的参数

更多：[API文档](https://harmony.pardeike.net/api/HarmonyLib.CodeInstructionExtensions.html)

## CodeMatcher

`CodeMatcher`用于寻找特定的指令集进行插入、删除或替换指令

例子：一个为 mod 提供事件的 API。基类 `DamageHandler` 管理伤害和死亡动画。 `DamageHandler.Apply()` 用于伤害处理，该方法调用的另一个方法 `DamageHandler.Kill()`，当角色死亡时调用。要将 `Kill()` 调用替换为一个 API 方法，该方法将触发 `OnDeath` 事件。由于 `Kill()` 方法还在其他 API 方法中使用，不希望触发其他API的事件，因此无法直接对 `Kill()` 进行补丁

这种情况下需要找到对`kill()`的调用，并将其替换为自己的方法`MyDeathHandler()`

`CodeMatcher.ThrowIfInvalid()` 会在代码不匹配时抛出异常，`ReportFailure`会返回一个布尔值

```csharp
[HarmonyPatch]
public static class DamageHandler_Apply_Patch
{
    // 参见 "辅助方法"
    static IEnumerable<MethodBase> TargetMethods()
    {
        var result = new List<MethodBase>();
        // ...（定位所有派生自 DamageHandler.Apply 的方法）
        return result;
    }

    static void MyDeathHandler(DamageHandler handler, Player player)
    {
        // ...
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions /*, ILGenerator generator*/)
    {
        // 没有 ILGenerator，CodeMatcher 将无法创建标签
        var codeMatcher = new CodeMatcher(instructions /*, ILGenerator generator*/);

        codeMatcher.MatchStartForward(
                CodeMatch.Calls(() => default(DamageHandler).Kill(default(Player)))
            )
            .ThrowIfInvalid("Could not find call to DamageHandler.Kill")
            .RemoveInstruction()
            .InsertAndAdvance(
                CodeInstruction.Call(() => MyDeathHandler(default, default))
            );

        return codeMatcher.Instructions();
    }
}
```

如果不是所有补丁方法都需要调用`kill()`，可以检查匹配情况，如果匹配失败 `CodeMatcher` 指针会停留在指令列表的末尾。使用 `Start()` 方法可以将光标返回到开头。

```csharp
static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions /*, ILGenerator generator*/){
    var codeMatcher = new CodeMatcher(instructions /*, ILGenerator generator*/);
    codeMatcher.MatchStartForward(
            CodeMatch.Calls(() => default(DamageHandler).Kill(default))
        );

    if (codeMatcher.IsValid)
    {
        codeMatcher.RemoveInstruction()
            .InsertAndAdvance(
                CodeInstruction.Call(() => MyDeathHandler(default, default))
            );
    }

    codeMatcher.Start();
    // 其他匹配...

	return codeMatcher.Instructions();
}
```

`Kill()` 方法可能会多次调用。可以使用 `CodeMatcher.Repeat()`，该方法会将当前的匹配代码传递给操作。如果没有匹配成功，可以定义一个可选操作，该操作接受一个错误消息作为参数，并在没有匹配发生时调用

```csharp
static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions /*, ILGenerator generator*/) {
    var codeMatcher = new CodeMatcher(instructions /*, ILGenerator generator*/);
    codeMatcher.MatchStartForward(CodeMatch.Calls(() => default(DamageHandler).Kill(default)))

               // 只获取最后一个匹配条件
               .Repeat(matchAction: cm => {
                                        cm.RemoveInstruction();
                                        cm.InsertAndAdvance(CodeInstruction.Call(() => MyDeathHandler(default,
                                                             default)));
                                    });

    return codeMatcher.Instructions();
}
```

# 注入参数

* `__instance`：用于访问实例对象的值
* `__result`：用于访问返回的值
* `__resultRef`：修改`ref 返回`本身，类型必须是`RefResult<T>`的引用，`T`必须与返回值类型匹配
* `__state`：在Prefix中存储信息，Postfix中访问，两个补丁必须在同一个类中
* `___fields`：三个下划线开头，可以用于访问/修改同名私有字段，例如`___example`可以用于访问`example`字段，若要修改需要使用ref
* `__args`：一次性访问所有参数
* `__runOriginal`：判断原始方法有没有执行过

# Reverse Patch
`Reverse Patch`将变成原始方法或原始方法的一部分，从而可以调用

例子：

```csharp
private class OriginalCode{
    private void Test(int counter, string name){
        // ...
    }
}

[HarmonyPatch]
public class Patch{
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(OriginalCode), "Test")]
    // 方法签名必须和原方法相同，PS：这里的第一个参数就是this，类的第一个参数是实例自身
    public static void MyTest(object instance, int counter, string name) =>
        // 这是一个存根，因此没有初始内容
        throw new NotImplementedException("It's a stub");
}

class Main
{
    void Test() =>
        // 调用 OriginalCode.Test()
        Patch.MyTest(originalInstance, 100, "hello");
}
```

## 修改原始方法
`Reverse Patch`最大的作用是可以使用`Transpiler`，它允许更改复制到目标类方法中的原始IL代码

例如：假设原始方法很长，并且有一部分用于计算校验和，与其从反编译器中复制源代码到自己的方法中，可以将包含校验和计算的原始方法反向补丁到自己的 Checksum(...) 方法中

```csharp
[HarmonyPatch]
public class Patch {
    // 使用Reverse Patch时，StringOperation 将包含原始代码的所有部分，包括 Join()，但不包括 +
    //
    // 基本上是：
    // var parts = original.Split('-').Reverse().ToArray();
    // return string.Join("", parts)
    //
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(OriginalClass), "SpecialCalculation")]
    public static string StringOperation(string original) {
        // 这个内部 transpiler 将应用于原始方法并且替换结果。
        // 这允许此方法具有不同于原始方法的签名，并且必须与 transpiler 的结果匹配
        //
        IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            // 操作指令序列，将指令操作码和操作数进行替换
            var list = Transpilers.Manipulator(instructions,
                                               item => item.opcode == OpCodes.Ldarg_1,
                                               item => item.opcode = OpCodes.Ldarg_0)
                                  .ToList();

            // 获取 string.Join 方法的信息
            var mJoin = SymbolExtensions.GetMethodInfo(() => string.Join(null, null));
            // 查找调用 string.Join 的指令索引
            var idx   = list.FindIndex(item => item.opcode == OpCodes.Call && item.operand as MethodInfo == mJoin);
            // 移除后续指令
            list.RemoveRange(idx + 1, list.Count - (idx + 1));
            return list.AsEnumerable();
        }
        
        // 调用 Transpiler 方法处理指令
        _ = Transpiler(null);
        return original;
    }
}
```

# 注解
Harmony基于注解开发，例如当调用`harmony.PatchAll()`时，会扫描程序集的所有类和方法，查找特定的`Harmony`注解，并应用到所有补丁类

```csharp
using HarmonyLib;

[HarmonyPatch(typeof(SomeTypeHere))]
[HarmonyPatch("SomeMethodName")]
class MyPatches{
    static void Postfix(/*...*/){
        //...
    }
}
```

这个例子中通过注解提供信息识别要补丁的方法，对于方法内的补丁，如果使用注解那么方法可以是别的名称

如果对一个类进行了多个补丁，并且希望它们按顺序执行，那么可以使用`HarmonyPriority`注解规定优先级

基本注解：
* 空注解：`[HarmonyPatch]`
* 类/类型注解：`[HarmonyPatch(Type declaringType)]`，使用类型注解定义包含要补丁的原始方法/属性/构造函数的类/类型
* 名称注解：`[HarmonyPatch(string methodName)]`，使用字符串注解定义方法或属性的名称，如果方法有重载可以添加参数类型数组`[HarmonyPatch(string methodName, params Type[] argumentTypes)]
`
* 方法类型注解：`[HarmonyPatch(MethodType methodType)]`，定义要补丁的方法类型（如 Method、Getter、Setter、Constructor）。

示例：
* 补丁 String.ToUpper() 方法：`[HarmonyPatch(typeof(String), "ToUpper")]`
* 补丁 MyClass 类中的 Account 属性的 setter：`[HarmonyPatch(typeof(MyClass),"Account",MethodType.Setter)]`
* 补丁 String.IndexOf(char, int) 方法：`[HarmonyPatch(typeof(String), "IndexOf", new Type[] { typeof(char), typeof(int) })]`
* 构造函数：
  * 默认构造函数：`[HarmonyPatch(typeof(TestClass), MethodType.Constructor)]`
  * 带有重载：`[HarmonyPatch(typeof(TestClass), MethodType.Constructor, new Type[] { typeof(int) })]`
* 泛型方法：开放的泛型方法无法补丁，只能补丁具体的`[HarmonyPatch(typeof(TestClass<string>), "AddItem")]`