namespace SpaceGame.Lib;
using Scriban;


public class AdapterBulider : IBulider
{
    Type _typeOld;
    Type _typeNew;

    public AdapterBulider(Type typeOld, Type typeNew)
    {
        _typeOld = typeOld;
        _typeNew = typeNew;
    }
    public string Bulid()
    {
        var propertiesNew = _typeNew.GetProperties().ToList();

        var templateString = @"public class {{new_type_name}}Adapter : {{new_type_name}} {

        {{old_type_name}} _obj;
    
        public {{new_type_name}}Adapter({{old_type_name}} obj) => _obj = obj;
    {{for property in (properties_new)}}
    public {{property.property_type.name}} {{property.name}}
    {
    {{if property.can_read}}
        get
        {
            return IoC.Resolve<{{property.property_type.name}}>(""Game.Get.Property"", ""{{property.name}}"", _obj);
        }{{end}}
    {{if property.can_write}}
        set
        {
            return IoC.Resolve<ICommand>(""Game.Set.Property"", ""{{property.name}}"", _obj, value).Execute();
        }{{end}}
    }
    {{end}}
    }";
        var template = Template.Parse(templateString);
        var result = template.Render(new
        {
            new_type_name = _typeNew.Name,
            old_type_name = _typeOld.Name,
            properties_new = propertiesNew,
        });
        return result;
    }
}
