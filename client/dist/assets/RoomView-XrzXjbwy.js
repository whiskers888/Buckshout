import{d as R,b6 as B,b7 as V,b8 as X,o,aB as a,W as e,b9 as k,ba as G,bb as O,bc as L,b as d,w as _,am as E,Q as b,a as t,Y as c,aE as p,bd as I,p as M,f as U,_ as N,be as H,bf as D,c as v,bg as j,bh as q,al as $,aC as A,V as T,bi as w,bj as C,aK as Y,bk as Q,a_ as W,bl as J}from"./index-uZEIanLt.js";import{V as g,e as Z,f as x}from"./VMain-Dnsgw4K8.js";import{V as ee}from"./transition-BbYqMF7u.js";import"./forwardRefs-BfRPK9L0.js";const z=r=>(M("data-v-b53c7662"),r=r(),U(),r),te={class:"item-container"},se={key:0,class:"item-tooltip"},oe={class:"item-behavior"},ie={key:0},le={class:"item-tooltip-description"},ae={key:0,class:"item-tooltip-lore"},re={key:1},ne=z(()=>t("h3",null,"Неизвестно",-1)),ce=z(()=>t("p",{class:"item-tooltip-description"},"Этот предмет скрыт от Вашего взора.",-1)),de=[ne,ce],_e=["src","alt"],ue=R({__name:"Item",props:{owner:{},item:{}},setup(r){const l=B(),n=V(),u=X();function i(){if(n.status!==I.READY){u.error("Дробовик еще не заряжен!");return}l.activity===L.USING_ITEM?l.selectAsTarget(h,m):l.beginUse(m)}const{owner:h,item:m}=r;return(s,y)=>{var S;return o(),a("div",te,[s.item&&!s.owner.is(e(k).PLAYER_DEAD)?(o(),a("div",{key:0,class:G(["item",{target:e(l).canTargetItem(s.owner,s.item),special:((S=s.item)==null?void 0:S.behavior.includes(e(O).CUSTOM))&&s.owner.id===e(l).id,active:e(l).activity===e(L).DECIDES_CANCEL}]),onClick:i},[d(g,{location:"right"},{activator:_(({props:P})=>[d(E,b(P,{class:"item-info",icon:"mdi-information"}),null,16)]),default:_(()=>[s.item.is(e(k).ITEM_INVISIBLE)?(o(),a("div",re,de)):(o(),a("div",se,[t("h3",null,c(s.item.name),1),t("div",oe,[t("div",null,"Тип: "+c(s.item.typeTooltip),1),t("div",null,"Способность: "+c(s.item.behaviorTooltip),1),s.item.behavior.includes(e(O).UNIT_TARGET)?(o(),a("div",ie," Цель: "+c(s.item.targetTooltip),1)):p("",!0)]),t("p",le,c(s.item.description),1),s.item.lore?(o(),a("p",ae,c(s.item.lore),1)):p("",!0)]))]),_:1}),t("img",{class:"item-model",src:s.item.is(e(k).ITEM_INVISIBLE)?"/models/items/unknown.png":`/models/items/${s.item.model}.png`,alt:s.item.is(e(k).ITEM_INVISIBLE)?"Неизвестно":s.item.name},null,8,_e)],2)):p("",!0)])}}}),pe=N(ue,[["__scopeId","data-v-b53c7662"]]),me=r=>(M("data-v-67866fce"),r=r(),U(),r),ye={class:"player-status"},ve={class:"player-info"},fe={class:"player-avatar"},he=["id","src"],ge={style:{"max-width":"100%"}},be={class:"player-name"},ke={key:0,class:"player-actions"},$e={class:"player-modifiers"},Ee=me(()=>t("b",null,"Статус:",-1)),Ie={class:"modifier-tooltip"},Ae={class:"item-tooltip-description"},Ce={class:"player-inventory"},Te=R({__name:"Player",props:{player:{}},setup(r){H(i=>({"29f66edd":i.player.color}));const l=D(),n=B(),u=V();return(i,h)=>{var m;return o(),a("div",{class:G(["player",{me:i.player.isOwnedByUser,current:i.player.isCurrent,target:e(n).canTargetPlayer(i.player),dead:i.player.is(e(k).PLAYER_DEAD)}]),style:w({borderColor:i.player.color})},[t("div",ye,[t("div",ve,[t("div",fe,[t("img",{id:`Player-${i.player.id}`,src:`https://api.multiavatar.com/${i.player.avatar}.png`,alt:"?"},null,8,he)]),t("div",ge,[t("div",be,[d(g,{location:"right",text:"Это Вы, единственный и неповторимый!"},{activator:_(({props:s})=>[i.player.isOwnedByUser?(o(),v(E,b({key:0},s,{color:i.player.color,icon:"mdi-star"}),null,16,["color"])):p("",!0)]),_:1}),d(g,{location:"right",text:i.player.isOwnedByUser?"И сейчас Ваш ход!":"Этот игрок сейчас ходит."},{activator:_(({props:s})=>[i.player.isCurrent?(o(),v(E,b({key:0},s,{color:i.player.color,icon:"mdi-shoe-sneaker"}),null,16,["color"])):p("",!0)]),_:1},8,["text"]),t("span",null,c(i.player.name),1)]),d(g,{location:"right",text:`Здоровье игрока: ${i.player.health}/${e(l).settings.MAX_PLAYER_HEALTH} ед.`},{activator:_(({props:s})=>[t("div",j(q(s)),[(o(!0),a($,null,A(e(l).settings.MAX_PLAYER_HEALTH,y=>(o(),v(E,{icon:"mdi-heart",color:y<=i.player.health?"#f00":"#000",key:y},null,8,["color"]))),128))],16)]),_:1},8,["text"])])]),e(n).isCurrent&&!i.player.is(e(k).PLAYER_DEAD)&&e(n).activity!==e(L).DECIDES_CANCEL?(o(),a("div",ke,[e(n).canTargetPlayer(i.player)?(o(),v(g,{key:0,location:"bottom",text:`Использовать [${(m=e(n).itemToUse)==null?void 0:m.name}]`},{activator:_(({props:s})=>[d(T,b(s,{icon:"mdi-teddy-bear",onClick:h[0]||(h[0]=y=>e(n).selectAsTarget(i.player))}),null,16)]),_:1},8,["text"])):(o(),a($,{key:1},[e(u).target!==i.player?(o(),v(g,{key:0,location:"bottom",text:"Прицелиться"},{activator:_(({props:s})=>[d(T,b(s,{icon:"mdi-target",disabled:e(u).status!==e(I).READY,onClick:h[1]||(h[1]=y=>e(l).invokeAim(i.player))}),null,16,["disabled"])]),_:1})):(o(),v(g,{key:1,location:"bottom",text:"Выстрелить"},{activator:_(({props:s})=>[d(T,b(s,{icon:"mdi-bullet",disabled:e(u).status!==e(I).READY,onClick:h[2]||(h[2]=y=>e(l).invokeShoot(i.player))}),null,16,["disabled"])]),_:1}))],64))])):p("",!0)]),t("div",$e,[Ee,(o(!0),a($,null,A(i.player.modifiers,s=>(o(),v(g,{key:s.name,location:"right"},{activator:_(({props:y})=>[d(E,b({ref_for:!0},y,{style:{border:`1px solid ${s.isBuff?"#0f0":"#f00"}`,borderRadius:"50%",padding:"12px",fontSize:"18px"},icon:`mdi-${s.icon}`}),null,16,["style","icon"])]),default:_(()=>[t("div",Ie,[t("h3",null,c(s.name),1),t("p",Ae,c(s.description),1)])]),_:2},1024))),128))]),t("div",Ce,[(o(!0),a($,null,A(e(l).settings.MAX_INVENTORY_SLOTS,s=>{var y;return o(),v(pe,{key:((y=i.player.inventory[s-1])==null?void 0:y.id)??s,owner:i.player,item:i.player.inventory[s-1]},null,8,["owner","item"])}),128))])],6)}}}),Re=N(Te,[["__scopeId","data-v-67866fce"]]),Se="/models/rifle/particles/smoke.gif",F="/models/rifle/particles/buck.png",K="/models/rifle/particles/blank.png",Le=r=>(M("data-v-610c0c23"),r=r(),U(),r),Ne={class:"rifle-container"},Pe={class:"rifle-model-container"},we={class:"rifle-modifiers"},Be={class:"modifier-tooltip"},Ve={class:"rifle-tooltip-description"},De={key:0},Ge=Le(()=>t("img",{class:"rifle-buckshot-smoke",src:Se},null,-1)),Me={key:0,class:"rifle-miss"},Ue=["src"],Oe={key:1},He={key:0,class:"rifle-bullet falling",src:F},Ye={key:1,class:"rifle-bullet falling",src:K},ze={key:2},Fe={key:0,class:"rifle-bullet",src:F},Ke={key:1,class:"rifle-bullet",src:K},Xe=R({__name:"Rifle",setup(r){H(i=>({"29d11b51":e(l).offset}));const l=V(),n=D(),u=B();return(i,h)=>(o(),a("div",Ne,[t("div",Pe,[t("div",we,[(o(!0),a($,null,A(e(l).modifiers,m=>(o(),v(g,{key:m.name,location:"bottom"},{activator:_(({props:s})=>[d(E,b({ref_for:!0},s,{style:{border:`1px solid ${m.isBuff?"#0f0":"#f00"}`,borderRadius:"50%",padding:"12px",fontSize:"18px"},icon:`mdi-${m.icon}`}),null,16,["style","icon"])]),default:_(()=>[t("div",Be,[t("h3",null,c(m.name),1),t("p",Ve,c(m.description),1)])]),_:2},1024))),128))]),e(l).status==e(I).SHOOTING&&e(l).patrons.current==e(C).CHARGED?(o(),a("div",De,[Ge,e(l).isMissing?(o(),a("div",Me," MMIS ")):p("",!0)])):p("",!0),t("img",{style:w({opacity:e(l).status==e(I).LOADING?.4:1}),class:G(["rifle-model",{"rifle-buckshot-kickback":e(l).status==e(I).SHOOTING&&e(l).patrons.current==e(C).CHARGED}]),src:e(l).is(e(k).RIFLE_BONUS_DAMAGE)?"/models/rifle/rifle_hacksawed.png":"/models/rifle/rifle.png"},null,14,Ue),t("div",null,[d(g,{location:"bottom",text:"Патроны заряжаются в случайном порядке, их кол-во скрыто во время раунда!"},{activator:_(({props:m})=>[t("div",b({class:"flex"},m),[(o(!0),a($,null,A(e(n).settings.MAX_PATRONS_IN_RIFLE,s=>(o(),v(E,{icon:"mdi-bullet",color:s<=e(l).patrons.sequence.length?e(l).patrons.sequence[s-1]?"#b60202":"#4949a3":"#3b3b3b",key:s},null,8,["color"]))),128))],16)]),_:1})]),e(l).status==e(I).PULLING?(o(),a("div",Oe,[e(l).patrons.current==e(C).CHARGED?(o(),a("img",He)):e(l).patrons.current==e(C).BLANK?(o(),a("img",Ye)):p("",!0)])):p("",!0),e(u).activity===e(L).CHECKING_RIFLE?(o(),a("div",ze,[e(l).patrons.current==e(C).CHARGED?(o(),a("img",Fe)):e(l).patrons.current==e(C).BLANK?(o(),a("img",Ke)):p("",!0)])):p("",!0)])]))}}),je=N(Xe,[["__scopeId","data-v-610c0c23"]]),qe={class:"game-toolbar"},Qe={key:1,style:{width:"100%"}},We={style:{display:"flex","justify-content":"space-between"}},Je={class:"flex"},Ze={class:"flex"},xe={key:0,class:"flex"},et={class:"chain-items"},tt={class:"chain-player"},st=["src"],ot={style:{display:"flex","flex-direction":"column","align-items":"center"}},it={class:"chain-affected-item"},lt=["src"],at={class:"clock"},rt={key:0,class:"chain-affected-item"},nt=["src"],ct={key:1,class:"chain-player"},dt=["src"],_t=R({__name:"Toolbar",setup(r){const l=Y(),n=D();V();const u=B(),i=Q();return(h,m)=>{var s,y,S,P;return o(),a("div",qe,[d(T,{color:"red",class:"mr-4",text:"Выйти",onClick:e(l).leave},null,8,["onClick"]),e(n).status===e(W).PREPARING?(o(),v(T,{key:0,text:"Начать игру",onClick:e(n).invokeStart},null,8,["onClick"])):(o(),a("div",Qe,[t("div",We,[t("div",Je,[t("div",{class:"current-player-color",style:w({background:(s=e(n).current)==null?void 0:s.color})},null,4),t("p",null,c(e(u).isCurrent?"Ваша очередь!":`Очередь: ${(y=e(n).current)==null?void 0:y.name}`)+", Осталось "+c(e(n).turn.time/1e3)+" сек. ",1)])]),t("div",Ze,[e(u).activity===e(L).USING_ITEM?(o(),a("div",xe,[t("p",null," Выберите цель ("+c((S=e(u).itemToUse)==null?void 0:S.targetTooltip)+"), чтобы применить ["+c((P=e(u).itemToUse)==null?void 0:P.name)+"]. ",1),d(T,{text:"Отмена",onClick:e(u).cancelUse},null,8,["onClick"])])):p("",!0),t("div",et,[(o(!0),a($,null,A(e(i).chain,f=>(o(),a("div",{key:f.item.id,class:"chain-item",style:w({borderColor:f.initiator.color})},[t("div",tt,[t("img",{src:`https://api.multiavatar.com/${f.initiator.avatar}.png`,alt:"?"},null,8,st),t("p",null,c(f.initiator.name),1)]),t("div",ot,[t("div",it,[t("img",{src:f.item.is(e(k).ITEM_INVISIBLE)?"/models/items/unknown.png":`/models/items/${f.item.model}.png`},null,8,lt)]),t("div",at,[t("p",null,c(f.time/1e3),1)])]),f.targetItem?(o(),a("div",rt,[t("img",{src:f.targetItem.is(e(k).ITEM_INVISIBLE)?"/models/items/unknown.png":`/models/items/${f.targetItem.model}.png`},null,8,nt)])):p("",!0),f.target?(o(),a("div",ct,[t("img",{src:`https://api.multiavatar.com/${f.target.avatar}.png`,alt:"?"},null,8,dt),t("p",null,c(f.target.name),1)])):p("",!0)],4))),128))])])]))])}}}),ut=N(_t,[["__scopeId","data-v-9afab7e7"]]),pt={class:"game-field"},mt={class:"players"},yt=R({__name:"Game",setup(r){const l=D();return(n,u)=>(o(),v(ee,{class:"game-container actions-disabled"},{default:_(()=>[d(ut),t("div",pt,[t("div",mt,[(o(!0),a($,null,A(e(l).players,i=>(o(),v(Re,{key:i.id,player:i},null,8,["player"]))),128))]),d(je)])]),_:1}))}}),vt=N(yt,[["__scopeId","data-v-5fac8be9"]]),kt=R({__name:"RoomView",setup(r){const l=Y();return J(()=>{console.log("LEAVE"),l.invokeLeave()}),(n,u)=>(o(),v(Z,null,{default:_(()=>[d(x,{scrollable:""},{default:_(()=>[d(vt)]),_:1})]),_:1}))}});export{kt as default};
