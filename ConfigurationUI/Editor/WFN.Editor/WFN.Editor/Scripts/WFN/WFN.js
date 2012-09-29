Type.registerNamespace("Tridion.Extensions.WFN.Commands");

Tridion.Extensions.WFN.Commands.DoSomething = function DoSomething(name, action) {
    Type.enableInterface(this, "Tridion.Extensions.WFN.Commands.DoSomething");
    this.addInterface("Tridion.Core.Command", ["DoSomething"]);
    
};

Tridion.Extensions.WFN.Commands.DoSomething.prototype._isEnabled = function DoSomething$_isEnabled(selection) {
    return true;
};

Tridion.Extensions.WFN.Commands.DoSomething.prototype._isAvailable = function DoSomething$_isAvailable(selection) {
    return true;
};

Tridion.Extensions.WFN.Commands.DoSomething.prototype._execute = function DoSomething$_execute(selection, pipeline) {
    alert('DoSomething$_execute');
};

