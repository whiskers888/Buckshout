import{j as I,m as R,aa as G,l as B,ah as J,n as T,b as t,F as k,t as E,x as K,y as X,B as O,a2 as Y,E as y,r as Z,L as C,aV as p,b3 as ee,az as ae,N as te,a6 as F,aj as ne,al as re,Q as le,I as se,a7 as ie,a5 as oe,q as ue,a9 as ce,b4 as ve,k as de,v as ge,ac as me,C as fe,z as Se,af as ye,ag as _e,b5 as be,am as he,H as ke,ak as Ve}from"./index-uZEIanLt.js";import{m as ze,M as h}from"./transition-BbYqMF7u.js";import{I as Pe}from"./index-CDJeLhzx.js";function Ce(e){return{aspectStyles:k(()=>{const u=Number(e.aspectRatio);return u?{paddingBottom:String(1/u*100)+"%"}:void 0})}}const H=I({aspectRatio:[String,Number],contentClass:null,inline:Boolean,...R(),...G()},"VResponsive"),W=B()({name:"VResponsive",props:H(),setup(e,u){let{slots:i}=u;const{aspectStyles:s}=Ce(e),{dimensionStyles:m}=J(e);return T(()=>{var d;return t("div",{class:["v-responsive",{"v-responsive--inline":e.inline},e.class],style:[m.value,e.style]},[t("div",{class:"v-responsive__sizer",style:s.value},null),(d=i.additional)==null?void 0:d.call(i),i.default&&t("div",{class:["v-responsive__content",e.contentClass]},[i.default()])])}),{}}}),Ie=I({absolute:Boolean,alt:String,cover:Boolean,color:String,draggable:{type:[Boolean,String],default:void 0},eager:Boolean,gradient:String,lazySrc:String,options:{type:Object,default:()=>({root:void 0,rootMargin:void 0,threshold:void 0})},sizes:String,src:{type:[String,Object],default:""},crossorigin:String,referrerpolicy:String,srcset:String,position:String,...H(),...R(),...E(),...ze()},"VImg"),Re=B()({name:"VImg",directives:{intersect:Pe},props:Ie(),emits:{loadstart:e=>!0,load:e=>!0,error:e=>!0},setup(e,u){let{emit:i,slots:s}=u;const{backgroundColorClasses:m,backgroundColorStyles:d}=K(X(e,"color")),{roundedClasses:V}=O(e),g=Y("VImg"),_=y(""),r=Z(),n=y(e.eager?"loading":"idle"),v=y(),b=y(),o=k(()=>e.src&&typeof e.src=="object"?{src:e.src.src,srcset:e.srcset||e.src.srcset,lazySrc:e.lazySrc||e.src.lazySrc,aspect:Number(e.aspectRatio||e.src.aspect||0)}:{src:e.src,srcset:e.srcset,lazySrc:e.lazySrc,aspect:Number(e.aspectRatio||0)}),f=k(()=>o.value.aspect||v.value/b.value||0);C(()=>e.src,()=>{z(n.value!=="idle")}),C(f,(a,l)=>{!a&&l&&r.value&&S(r.value)}),p(()=>z());function z(a){if(!(e.eager&&a)&&!(ee&&!a&&!e.eager)){if(n.value="loading",o.value.lazySrc){const l=new Image;l.src=o.value.lazySrc,S(l,null)}o.value.src&&ae(()=>{var l;i("loadstart",((l=r.value)==null?void 0:l.currentSrc)||o.value.src),setTimeout(()=>{var c;if(!g.isUnmounted)if((c=r.value)!=null&&c.complete){if(r.value.naturalWidth||N(),n.value==="error")return;f.value||S(r.value,null),n.value==="loading"&&w()}else f.value||S(r.value),j()})})}}function w(){var a;g.isUnmounted||(j(),S(r.value),n.value="loaded",i("load",((a=r.value)==null?void 0:a.currentSrc)||o.value.src))}function N(){var a;g.isUnmounted||(n.value="error",i("error",((a=r.value)==null?void 0:a.currentSrc)||o.value.src))}function j(){const a=r.value;a&&(_.value=a.currentSrc||a.src)}let P=-1;te(()=>{clearTimeout(P)});function S(a){let l=arguments.length>1&&arguments[1]!==void 0?arguments[1]:100;const c=()=>{if(clearTimeout(P),g.isUnmounted)return;const{naturalHeight:U,naturalWidth:x}=a;U||x?(v.value=x,b.value=U):!a.complete&&n.value==="loading"&&l!=null?P=window.setTimeout(c,l):(a.currentSrc.endsWith(".svg")||a.currentSrc.startsWith("data:image/svg+xml"))&&(v.value=1,b.value=1)};c()}const A=k(()=>({"v-img__img--cover":e.cover,"v-img__img--contain":!e.cover})),M=()=>{var c;if(!o.value.src||n.value==="idle")return null;const a=t("img",{class:["v-img__img",A.value],style:{objectPosition:e.position},src:o.value.src,srcset:o.value.srcset,alt:e.alt,crossorigin:e.crossorigin,referrerpolicy:e.referrerpolicy,draggable:e.draggable,sizes:e.sizes,ref:r,onLoad:w,onError:N},null),l=(c=s.sources)==null?void 0:c.call(s);return t(h,{transition:e.transition,appear:!0},{default:()=>[F(l?t("picture",{class:"v-img__picture"},[l,a]):a,[[ie,n.value==="loaded"]])]})},q=()=>t(h,{transition:e.transition},{default:()=>[o.value.lazySrc&&n.value!=="loaded"&&t("img",{class:["v-img__img","v-img__img--preload",A.value],style:{objectPosition:e.position},src:o.value.lazySrc,alt:e.alt,crossorigin:e.crossorigin,referrerpolicy:e.referrerpolicy,draggable:e.draggable},null)]}),L=()=>s.placeholder?t(h,{transition:e.transition,appear:!0},{default:()=>[(n.value==="loading"||n.value==="error"&&!s.error)&&t("div",{class:"v-img__placeholder"},[s.placeholder()])]}):null,Q=()=>s.error?t(h,{transition:e.transition,appear:!0},{default:()=>[n.value==="error"&&t("div",{class:"v-img__error"},[s.error()])]}):null,$=()=>e.gradient?t("div",{class:"v-img__gradient",style:{backgroundImage:`linear-gradient(${e.gradient})`}},null):null,D=y(!1);{const a=C(f,l=>{l&&(requestAnimationFrame(()=>{requestAnimationFrame(()=>{D.value=!0})}),a())})}return T(()=>{const a=W.filterProps(e);return F(t(W,le({class:["v-img",{"v-img--absolute":e.absolute,"v-img--booting":!D.value},m.value,V.value,e.class],style:[{width:se(e.width==="auto"?v.value:e.width)},d.value,e.style]},a,{aspectRatio:f.value,"aria-label":e.alt,role:e.alt?"img":void 0}),{additional:()=>t(re,null,[t(M,null,null),t(q,null,null),t($,null,null),t(L,null,null),t(Q,null,null)]),default:s.default}),[[ne("intersect"),{handler:z,options:e.options},null,{once:!0}]])}),{currentSrc:_,image:r,state:n,naturalWidth:v,naturalHeight:b}}}),Be=I({start:Boolean,end:Boolean,icon:oe,image:String,text:String,...ue(),...R(),...ce(),...E(),...ve(),...de(),...ge(),...me({variant:"flat"})},"VAvatar"),je=B()({name:"VAvatar",props:Be(),setup(e,u){let{slots:i}=u;const{themeClasses:s}=fe(e),{borderClasses:m}=Se(e),{colorClasses:d,colorStyles:V,variantClasses:g}=ye(e),{densityClasses:_}=_e(e),{roundedClasses:r}=O(e),{sizeClasses:n,sizeStyles:v}=be(e);return T(()=>t(e.tag,{class:["v-avatar",{"v-avatar--start":e.start,"v-avatar--end":e.end},s.value,m.value,d.value,_.value,r.value,n.value,g.value,e.class],style:[V.value,v.value,e.style]},{default:()=>[i.default?t(ke,{key:"content-defaults",defaults:{VImg:{cover:!0,src:e.image},VIcon:{icon:e.icon}}},{default:()=>[i.default()]}):e.image?t(Re,{key:"image",src:e.image,alt:"",cover:!0},null):e.icon?t(he,{key:"icon",icon:e.icon},null):e.text,Ve(!1,"v-avatar")]})),{}}});export{Re as V,je as a};