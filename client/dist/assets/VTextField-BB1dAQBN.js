import{j as w,O as J,F as v,E as ee,r as R,L as Q,$ as Me,aL as we,y as oe,Z as De,m as j,l as T,n as O,b as t,aM as re,a6 as ne,a7 as de,a8 as W,v as te,aN as $e,am as Re,aO as ce,a5 as X,aP as Ae,t as pe,C as ve,aQ as Ee,B as Le,D as fe,a3 as le,x as Te,an as me,I as Oe,aR as ze,aS as Ne,H as Ue,al as Z,Q as q,aT as We,aU as je,a1 as G,a2 as Ke,W as He,aV as Qe,N as Xe,M as Ye,P as se,az as ae,a9 as Ze,aW as qe,aa as Ge,ag as Je,ah as ea,aX as aa,aj as na,aY as ta,aZ as la}from"./index-uZEIanLt.js";import{f as ge,n as ia,a as sa,s as ua}from"./forwardRefs-BfRPK9L0.js";import{m as ye,M as be}from"./transition-BbYqMF7u.js";import{I as oa}from"./index-CDJeLhzx.js";const he=Symbol.for("vuetify:form"),ra=w({disabled:Boolean,fastFail:Boolean,readonly:Boolean,modelValue:{type:Boolean,default:null},validateOn:{type:String,default:"input"}},"form");function da(e){const c=J(e,"modelValue"),i=v(()=>e.disabled),l=v(()=>e.readonly),n=ee(!1),a=R([]),r=R([]);async function V(){const u=[];let d=!0;r.value=[],n.value=!0;for(const f of a.value){const o=await f.validate();if(o.length>0&&(d=!1,u.push({id:f.id,errorMessages:o})),!d&&e.fastFail)break}return r.value=u,n.value=!1,{valid:d,errors:r.value}}function C(){a.value.forEach(u=>u.reset())}function g(){a.value.forEach(u=>u.resetValidation())}return Q(a,()=>{let u=0,d=0;const f=[];for(const o of a.value)o.isValid===!1?(d++,f.push({id:o.id,errorMessages:o.errorMessages})):o.isValid===!0&&u++;r.value=f,c.value=d>0?!1:u===a.value.length?!0:null},{deep:!0,flush:"post"}),Me(he,{register:u=>{let{id:d,vm:f,validate:o,reset:x,resetValidation:P}=u;a.value.some(A=>A.id===d),a.value.push({id:d,validate:o,reset:x,resetValidation:P,vm:we(f),isValid:null,errorMessages:[]})},unregister:u=>{a.value=a.value.filter(d=>d.id!==u)},update:(u,d,f)=>{const o=a.value.find(x=>x.id===u);o&&(o.isValid=d,o.errorMessages=f)},isDisabled:i,isReadonly:l,isValidating:n,isValid:c,items:a,validateOn:oe(e,"validateOn")}),{errors:r,isDisabled:i,isReadonly:l,isValidating:n,isValid:c,items:a,validate:V,reset:C,resetValidation:g}}function ca(){return De(he,null)}const va=w({...j(),...ra()},"VForm"),wa=T()({name:"VForm",props:va(),emits:{"update:modelValue":e=>!0,submit:e=>!0},setup(e,c){let{slots:i,emit:l}=c;const n=da(e),a=R();function r(C){C.preventDefault(),n.reset()}function V(C){const g=C,u=n.validate();g.then=u.then.bind(u),g.catch=u.catch.bind(u),g.finally=u.finally.bind(u),l("submit",g),g.defaultPrevented||u.then(d=>{var o;let{valid:f}=d;f&&((o=a.value)==null||o.submit())}),g.preventDefault()}return O(()=>{var C;return t("form",{ref:a,class:["v-form",e.class],style:e.style,novalidate:!0,onReset:r,onSubmit:V},[(C=i.default)==null?void 0:C.call(i,n)])}),ge(n,a)}}),fa=w({active:Boolean,disabled:Boolean,max:[Number,String],value:{type:[Number,String],default:0},...j(),...ye({transition:{component:re}})},"VCounter"),ma=T()({name:"VCounter",functional:!0,props:fa(),setup(e,c){let{slots:i}=c;const l=v(()=>e.max?`${e.value} / ${e.max}`:String(e.value));return O(()=>t(be,{transition:e.transition},{default:()=>[ne(t("div",{class:["v-counter",{"text-error":e.max&&!e.disabled&&parseFloat(e.value)>parseFloat(e.max)},e.class],style:e.style},[i.default?i.default({counter:l.value,max:e.max,value:e.value}):l.value]),[[de,e.active]])]})),{}}}),ga=w({text:String,onClick:W(),...j(),...te()},"VLabel"),ya=T()({name:"VLabel",props:ga(),setup(e,c){let{slots:i}=c;return O(()=>{var l;return t("label",{class:["v-label",{"v-label--clickable":!!e.onClick},e.class],style:e.style,onClick:e.onClick},[e.text,(l=i.default)==null?void 0:l.call(i)])}),{}}}),ba=w({floating:Boolean,...j()},"VFieldLabel"),Y=T()({name:"VFieldLabel",props:ba(),setup(e,c){let{slots:i}=c;return O(()=>t(ya,{class:["v-field-label",{"v-field-label--floating":e.floating},e.class],style:e.style,"aria-hidden":e.floating||void 0},i)),{}}});function Ve(e){const{t:c}=$e();function i(l){let{name:n}=l;const a={prepend:"prependAction",prependInner:"prependAction",append:"appendAction",appendInner:"appendAction",clear:"clear"}[n],r=e[`onClick:${n}`],V=r&&a?c(`$vuetify.input.${a}`,e.label??""):void 0;return t(Re,{icon:e[`${n}Icon`],"aria-label":V,onClick:r},null)}return{InputIcon:i}}const Ce=w({focused:Boolean,"onUpdate:focused":W()},"focus");function xe(e){let c=arguments.length>1&&arguments[1]!==void 0?arguments[1]:ce();const i=J(e,"focused"),l=v(()=>({[`${c}--focused`]:i.value}));function n(){i.value=!0}function a(){i.value=!1}return{focusClasses:l,isFocused:i,focus:n,blur:a}}const ha=["underlined","outlined","filled","solo","solo-inverted","solo-filled","plain"],ke=w({appendInnerIcon:X,bgColor:String,clearable:Boolean,clearIcon:{type:X,default:"$clear"},active:Boolean,centerAffix:{type:Boolean,default:void 0},color:String,baseColor:String,dirty:Boolean,disabled:{type:Boolean,default:null},error:Boolean,flat:Boolean,label:String,persistentClear:Boolean,prependInnerIcon:X,reverse:Boolean,singleLine:Boolean,variant:{type:String,default:"filled",validator:e=>ha.includes(e)},"onClick:clear":W(),"onClick:appendInner":W(),"onClick:prependInner":W(),...j(),...Ae(),...pe(),...te()},"VField"),Ie=T()({name:"VField",inheritAttrs:!1,props:{id:String,...Ce(),...ke()},emits:{"update:focused":e=>!0,"update:modelValue":e=>!0},setup(e,c){let{attrs:i,emit:l,slots:n}=c;const{themeClasses:a}=ve(e),{loaderClasses:r}=Ee(e),{focusClasses:V,isFocused:C,focus:g,blur:u}=xe(e),{InputIcon:d}=Ve(e),{roundedClasses:f}=Le(e),{rtlClasses:o}=fe(),x=v(()=>e.dirty||e.active),P=v(()=>!e.singleLine&&!!(e.label||n.label)),A=le(),y=v(()=>e.id||`input-${A}`),p=v(()=>`${y.value}-messages`),D=R(),k=R(),m=R(),s=v(()=>["plain","underlined"].includes(e.variant)),{backgroundColorClasses:I,backgroundColorStyles:S}=Te(oe(e,"bgColor")),{textColorClasses:b,textColorStyles:L}=me(v(()=>e.error||e.disabled?void 0:x.value&&C.value?e.color:e.baseColor));Q(x,h=>{if(P.value){const _=D.value.$el,F=k.value.$el;requestAnimationFrame(()=>{const B=ia(_),M=F.getBoundingClientRect(),H=M.x-B.x,z=M.y-B.y-(B.height/2-M.height/2),N=M.width/.75,U=Math.abs(N-B.width)>1?{maxWidth:Oe(N)}:void 0,_e=getComputedStyle(_),ie=getComputedStyle(F),Pe=parseFloat(_e.transitionDuration)*1e3||150,Fe=parseFloat(ie.getPropertyValue("--v-field-label-scale")),Be=ie.getPropertyValue("color");_.style.visibility="visible",F.style.visibility="hidden",sa(_,{transform:`translate(${H}px, ${z}px) scale(${Fe})`,color:Be,...U},{duration:Pe,easing:ua,direction:h?"normal":"reverse"}).finished.then(()=>{_.style.removeProperty("visibility"),F.style.removeProperty("visibility")})})}},{flush:"post"});const E=v(()=>({isActive:x,isFocused:C,controlRef:m,blur:u,focus:g}));function K(h){h.target!==document.activeElement&&h.preventDefault()}function $(h){var _;h.key!=="Enter"&&h.key!==" "||(h.preventDefault(),h.stopPropagation(),(_=e["onClick:clear"])==null||_.call(e,new MouseEvent("click")))}return O(()=>{var H,z,N;const h=e.variant==="outlined",_=!!(n["prepend-inner"]||e.prependInnerIcon),F=!!(e.clearable||n.clear),B=!!(n["append-inner"]||e.appendInnerIcon||F),M=()=>n.label?n.label({...E.value,label:e.label,props:{for:y.value}}):e.label;return t("div",q({class:["v-field",{"v-field--active":x.value,"v-field--appended":B,"v-field--center-affix":e.centerAffix??!s.value,"v-field--disabled":e.disabled,"v-field--dirty":e.dirty,"v-field--error":e.error,"v-field--flat":e.flat,"v-field--has-background":!!e.bgColor,"v-field--persistent-clear":e.persistentClear,"v-field--prepended":_,"v-field--reverse":e.reverse,"v-field--single-line":e.singleLine,"v-field--no-label":!M(),[`v-field--variant-${e.variant}`]:!0},a.value,I.value,V.value,r.value,f.value,o.value,e.class],style:[S.value,e.style],onClick:K},i),[t("div",{class:"v-field__overlay"},null),t(ze,{name:"v-field",active:!!e.loading,color:e.error?"error":typeof e.loading=="string"?e.loading:e.color},{default:n.loader}),_&&t("div",{key:"prepend",class:"v-field__prepend-inner"},[e.prependInnerIcon&&t(d,{key:"prepend-icon",name:"prependInner"},null),(H=n["prepend-inner"])==null?void 0:H.call(n,E.value)]),t("div",{class:"v-field__field","data-no-activator":""},[["filled","solo","solo-inverted","solo-filled"].includes(e.variant)&&P.value&&t(Y,{key:"floating-label",ref:k,class:[b.value],floating:!0,for:y.value,style:L.value},{default:()=>[M()]}),t(Y,{ref:D,for:y.value},{default:()=>[M()]}),(z=n.default)==null?void 0:z.call(n,{...E.value,props:{id:y.value,class:"v-field__input","aria-describedby":p.value},focus:g,blur:u})]),F&&t(Ne,{key:"clear"},{default:()=>[ne(t("div",{class:"v-field__clearable",onMousedown:U=>{U.preventDefault(),U.stopPropagation()}},[t(Ue,{defaults:{VIcon:{icon:e.clearIcon}}},{default:()=>[n.clear?n.clear({...E.value,props:{onKeydown:$,onFocus:g,onBlur:u,onClick:e["onClick:clear"]}}):t(d,{name:"clear",onKeydown:$,onFocus:g,onBlur:u},null)]})]),[[de,e.dirty]])]}),B&&t("div",{key:"append",class:"v-field__append-inner"},[(N=n["append-inner"])==null?void 0:N.call(n,E.value),e.appendInnerIcon&&t(d,{key:"append-icon",name:"appendInner"},null)]),t("div",{class:["v-field__outline",b.value],style:L.value},[h&&t(Z,null,[t("div",{class:"v-field__outline__start"},null),P.value&&t("div",{class:"v-field__outline__notch"},[t(Y,{ref:k,floating:!0,for:y.value},{default:()=>[M()]})]),t("div",{class:"v-field__outline__end"},null)]),s.value&&P.value&&t(Y,{ref:k,floating:!0,for:y.value},{default:()=>[M()]})])])}),{controlRef:m}}});function Va(e){const c=Object.keys(Ie.props).filter(i=>!We(i)&&i!=="class"&&i!=="style");return je(e,c)}const Ca=w({active:Boolean,color:String,messages:{type:[Array,String],default:()=>[]},...j(),...ye({transition:{component:re,leaveAbsolute:!0,group:!0}})},"VMessages"),xa=T()({name:"VMessages",props:Ca(),setup(e,c){let{slots:i}=c;const l=v(()=>G(e.messages)),{textColorClasses:n,textColorStyles:a}=me(v(()=>e.color));return O(()=>t(be,{transition:e.transition,tag:"div",class:["v-messages",n.value,e.class],style:[a.value,e.style],role:"alert","aria-live":"polite"},{default:()=>[e.active&&l.value.map((r,V)=>t("div",{class:"v-messages__message",key:`${V}-${l.value}`},[i.message?i.message({message:r}):r]))]})),{}}}),ka=w({disabled:{type:Boolean,default:null},error:Boolean,errorMessages:{type:[Array,String],default:()=>[]},maxErrors:{type:[Number,String],default:1},name:String,label:String,readonly:{type:Boolean,default:null},rules:{type:Array,default:()=>[]},modelValue:null,validateOn:String,validationValue:null,...Ce()},"validation");function Ia(e){let c=arguments.length>1&&arguments[1]!==void 0?arguments[1]:ce(),i=arguments.length>2&&arguments[2]!==void 0?arguments[2]:le();const l=J(e,"modelValue"),n=v(()=>e.validationValue===void 0?l.value:e.validationValue),a=ca(),r=R([]),V=ee(!0),C=v(()=>!!(G(l.value===""?null:l.value).length||G(n.value===""?null:n.value).length)),g=v(()=>!!(e.disabled??(a==null?void 0:a.isDisabled.value))),u=v(()=>!!(e.readonly??(a==null?void 0:a.isReadonly.value))),d=v(()=>{var m;return(m=e.errorMessages)!=null&&m.length?G(e.errorMessages).concat(r.value).slice(0,Math.max(0,+e.maxErrors)):r.value}),f=v(()=>{let m=(e.validateOn??(a==null?void 0:a.validateOn.value))||"input";m==="lazy"&&(m="input lazy"),m==="eager"&&(m="input eager");const s=new Set((m==null?void 0:m.split(" "))??[]);return{input:s.has("input"),blur:s.has("blur")||s.has("input")||s.has("invalid-input"),invalidInput:s.has("invalid-input"),lazy:s.has("lazy"),eager:s.has("eager")}}),o=v(()=>{var m;return e.error||(m=e.errorMessages)!=null&&m.length?!1:e.rules.length?V.value?r.value.length||f.value.lazy?null:!0:!r.value.length:!0}),x=ee(!1),P=v(()=>({[`${c}--error`]:o.value===!1,[`${c}--dirty`]:C.value,[`${c}--disabled`]:g.value,[`${c}--readonly`]:u.value})),A=Ke("validation"),y=v(()=>e.name??He(i));Qe(()=>{a==null||a.register({id:y.value,vm:A,validate:k,reset:p,resetValidation:D})}),Xe(()=>{a==null||a.unregister(y.value)}),Ye(async()=>{f.value.lazy||await k(!f.value.eager),a==null||a.update(y.value,o.value,d.value)}),se(()=>f.value.input||f.value.invalidInput&&o.value===!1,()=>{Q(n,()=>{if(n.value!=null)k();else if(e.focused){const m=Q(()=>e.focused,s=>{s||k(),m()})}})}),se(()=>f.value.blur,()=>{Q(()=>e.focused,m=>{m||k()})}),Q([o,d],()=>{a==null||a.update(y.value,o.value,d.value)});async function p(){l.value=null,await ae(),await D()}async function D(){V.value=!0,f.value.lazy?r.value=[]:await k(!f.value.eager)}async function k(){let m=arguments.length>0&&arguments[0]!==void 0?arguments[0]:!1;const s=[];x.value=!0;for(const I of e.rules){if(s.length>=+(e.maxErrors??1))break;const b=await(typeof I=="function"?I:()=>I)(n.value);if(b!==!0){if(b!==!1&&typeof b!="string"){console.warn(`${b} is not a valid value. Rule functions must return boolean true or a string.`);continue}s.push(b||"")}}return r.value=s,x.value=!1,V.value=m,r.value}return{errorMessages:d,isDirty:C,isDisabled:g,isReadonly:u,isPristine:V,isValid:o,isValidating:x,reset:p,resetValidation:D,validate:k,validationClasses:P}}const Se=w({id:String,appendIcon:X,centerAffix:{type:Boolean,default:!0},prependIcon:X,hideDetails:[Boolean,String],hideSpinButtons:Boolean,hint:String,persistentHint:Boolean,messages:{type:[Array,String],default:()=>[]},direction:{type:String,default:"horizontal",validator:e=>["horizontal","vertical"].includes(e)},"onClick:prepend":W(),"onClick:append":W(),...j(),...Ze(),...qe(Ge(),["maxWidth","minWidth","width"]),...te(),...ka()},"VInput"),ue=T()({name:"VInput",props:{...Se()},emits:{"update:modelValue":e=>!0},setup(e,c){let{attrs:i,slots:l,emit:n}=c;const{densityClasses:a}=Je(e),{dimensionStyles:r}=ea(e),{themeClasses:V}=ve(e),{rtlClasses:C}=fe(),{InputIcon:g}=Ve(e),u=le(),d=v(()=>e.id||`input-${u}`),f=v(()=>`${d.value}-messages`),{errorMessages:o,isDirty:x,isDisabled:P,isReadonly:A,isPristine:y,isValid:p,isValidating:D,reset:k,resetValidation:m,validate:s,validationClasses:I}=Ia(e,"v-input",d),S=v(()=>({id:d,messagesId:f,isDirty:x,isDisabled:P,isReadonly:A,isPristine:y,isValid:p,isValidating:D,reset:k,resetValidation:m,validate:s})),b=v(()=>{var L;return(L=e.errorMessages)!=null&&L.length||!y.value&&o.value.length?o.value:e.hint&&(e.persistentHint||e.focused)?e.hint:e.messages});return O(()=>{var h,_,F,B;const L=!!(l.prepend||e.prependIcon),E=!!(l.append||e.appendIcon),K=b.value.length>0,$=!e.hideDetails||e.hideDetails==="auto"&&(K||!!l.details);return t("div",{class:["v-input",`v-input--${e.direction}`,{"v-input--center-affix":e.centerAffix,"v-input--hide-spin-buttons":e.hideSpinButtons},a.value,V.value,C.value,I.value,e.class],style:[r.value,e.style]},[L&&t("div",{key:"prepend",class:"v-input__prepend"},[(h=l.prepend)==null?void 0:h.call(l,S.value),e.prependIcon&&t(g,{key:"prepend-icon",name:"prepend"},null)]),l.default&&t("div",{class:"v-input__control"},[(_=l.default)==null?void 0:_.call(l,S.value)]),E&&t("div",{key:"append",class:"v-input__append"},[e.appendIcon&&t(g,{key:"append-icon",name:"append"},null),(F=l.append)==null?void 0:F.call(l,S.value)]),$&&t("div",{class:"v-input__details"},[t(xa,{id:f.value,active:K,messages:b.value},{message:l.message}),(B=l.details)==null?void 0:B.call(l,S.value)])])}),{reset:k,resetValidation:m,validate:s,isValid:p,errorMessages:o}}}),Sa=["color","file","time","date","datetime-local","week","month"],_a=w({autofocus:Boolean,counter:[Boolean,Number,String],counterValue:[Number,Function],prefix:String,placeholder:String,persistentPlaceholder:Boolean,persistentCounter:Boolean,suffix:String,role:String,type:{type:String,default:"text"},modelModifiers:Object,...Se(),...ke()},"VTextField"),Da=T()({name:"VTextField",directives:{Intersect:oa},inheritAttrs:!1,props:_a(),emits:{"click:control":e=>!0,"mousedown:control":e=>!0,"update:focused":e=>!0,"update:modelValue":e=>!0},setup(e,c){let{attrs:i,emit:l,slots:n}=c;const a=J(e,"modelValue"),{isFocused:r,focus:V,blur:C}=xe(e),g=v(()=>typeof e.counterValue=="function"?e.counterValue(a.value):typeof e.counterValue=="number"?e.counterValue:(a.value??"").toString().length),u=v(()=>{if(i.maxlength)return i.maxlength;if(!(!e.counter||typeof e.counter!="number"&&typeof e.counter!="string"))return e.counter}),d=v(()=>["plain","underlined"].includes(e.variant));function f(s,I){var S,b;!e.autofocus||!s||(b=(S=I[0].target)==null?void 0:S.focus)==null||b.call(S)}const o=R(),x=R(),P=R(),A=v(()=>Sa.includes(e.type)||e.persistentPlaceholder||r.value||e.active);function y(){var s;P.value!==document.activeElement&&((s=P.value)==null||s.focus()),r.value||V()}function p(s){l("mousedown:control",s),s.target!==P.value&&(y(),s.preventDefault())}function D(s){y(),l("click:control",s)}function k(s){s.stopPropagation(),y(),ae(()=>{a.value=null,la(e["onClick:clear"],s)})}function m(s){var S;const I=s.target;if(a.value=I.value,(S=e.modelModifiers)!=null&&S.trim&&["text","search","password","tel","url"].includes(e.type)){const b=[I.selectionStart,I.selectionEnd];ae(()=>{I.selectionStart=b[0],I.selectionEnd=b[1]})}}return O(()=>{const s=!!(n.counter||e.counter!==!1&&e.counter!=null),I=!!(s||n.details),[S,b]=aa(i),{modelValue:L,...E}=ue.filterProps(e),K=Va(e);return t(ue,q({ref:o,modelValue:a.value,"onUpdate:modelValue":$=>a.value=$,class:["v-text-field",{"v-text-field--prefixed":e.prefix,"v-text-field--suffixed":e.suffix,"v-input--plain-underlined":d.value},e.class],style:e.style},S,E,{centerAffix:!d.value,focused:r.value}),{...n,default:$=>{let{id:h,isDisabled:_,isDirty:F,isReadonly:B,isValid:M}=$;return t(Ie,q({ref:x,onMousedown:p,onClick:D,"onClick:clear":k,"onClick:prependInner":e["onClick:prependInner"],"onClick:appendInner":e["onClick:appendInner"],role:e.role},K,{id:h.value,active:A.value||F.value,dirty:F.value||e.dirty,disabled:_.value,focused:r.value,error:M.value===!1}),{...n,default:H=>{let{props:{class:z,...N}}=H;const U=ne(t("input",q({ref:P,value:a.value,onInput:m,autofocus:e.autofocus,readonly:B.value,disabled:_.value,name:e.name,placeholder:e.placeholder,size:1,type:e.type,onFocus:y,onBlur:C},N,b),null),[[na("intersect"),{handler:f},null,{once:!0}]]);return t(Z,null,[e.prefix&&t("span",{class:"v-text-field__prefix"},[t("span",{class:"v-text-field__prefix__text"},[e.prefix])]),n.default?t("div",{class:z,"data-no-activator":""},[n.default(),U]):ta(U,{class:z}),e.suffix&&t("span",{class:"v-text-field__suffix"},[t("span",{class:"v-text-field__suffix__text"},[e.suffix])])])}})},details:I?$=>{var h;return t(Z,null,[(h=n.details)==null?void 0:h.call(n,$),s&&t(Z,null,[t("span",null,null),t(ma,{active:e.persistentCounter||r.value,value:g.value,max:u.value,disabled:e.disabled},n.counter)])])}:void 0})}),ge({},o,x,P)}});export{Da as V,wa as a};
