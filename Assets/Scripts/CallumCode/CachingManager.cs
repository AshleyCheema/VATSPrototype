using System;
using System.Collections.Generic;

namespace CallumCode
{
    public sealed class CachingManager
    {
        Dictionary<object, int> m_IDCache = new Dictionary<object, int>();

        // Caching method takes the string value of an enum and tries to get a value from Unity
        public void Init(Type enumeratedValues, Func<string, int> cachingMethod)
        {
            Array values = enumeratedValues.GetEnumValues();
            foreach (var enumVal in values)
            {
                // Mapping the enum name to parameter hash
                m_IDCache[enumVal] = cachingMethod.Invoke(enumVal.ToString());
            }
        }

        public int this[Enum enumVal]
        {
            get
            {
                return m_IDCache[enumVal];
            }

            set
            {
                m_IDCache[enumVal] = value;
            }
        }
    }

    /*
     * This caching manager will relate the underlying value of an 
     * enum to a value that the cachingMethod can handle using the 
     * string value of that enum. For example:
     * 
     * My shader has a value called 'FillProgress'. To set this value
     * my enum will look like this:
     * 
     * enum ShaderValues
     * {
     *     FillProgress,
     *     Alpha
     *     Green
     * }
     * 
     * The typed definitions of the enum must match the variable name
     * including case.
     * 
     * Init is then called using this enums type: 
     *      Init(typeof(ShaderValues), cachingMethod) 
     * This caching method is likely to be something like Shader.PropertyToID
     * or Animator.StringToHash
     * 
     * To use this ID later, access with [] and use the enum value for the 
     * property you're trying to get the index for. For example:
     * 
     * m_animator.SetTrigger(m_cache[AnimatorValues.OnShoot])
     * opposed to...
     * m_animator.SetTrigger("OnShoot");
     */

}
