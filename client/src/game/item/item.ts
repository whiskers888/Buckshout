import { Modifier, type ModifierState } from '@/game/modifier/modifier';
import { useGame } from '@/stores/game';

export enum ItemBehavior {
	NO_TARGET,
	UNIT_TARGET,
	IMMEDIATE,
	CUSTOM,
}

export enum UnitTargetType {
	NONE,
	PLAYER,
	ITEM,
}

export enum UnitTargetTeam {
	NONE,
	FRIENDLY,
	ENEMY,
	ANY,
}

export enum ItemType {
	DEFAULT,
	TRAP,
}

export enum ItemModifier {
	CANNOT_BE_STOLEN,
	INVISIBLE,
}

export enum ItemEvent {
	USED = 'USED',
	EFFECTED = 'EFFECTED',
	CANCELED = 'CANCELED',
}

const behaviorTooltip = {
	[ItemBehavior.NO_TARGET]: 'ненаправленная',
	[ItemBehavior.UNIT_TARGET]: 'направленная',
	[ItemBehavior.IMMEDIATE]: 'мгновенного применения',
	[ItemBehavior.CUSTOM]: '',
};

const itemTypeTooltip = {
	[ItemType.DEFAULT]: 'обычный',
	[ItemType.TRAP]: 'ловушка',
};

const targetTypeTooltip = {
	[UnitTargetType.NONE]: '',
	[UnitTargetType.ITEM]: 'предмет',
	[UnitTargetType.PLAYER]: 'игрок',
};

const targetTeamTooltip = {
	[UnitTargetTeam.NONE]: '',
	[UnitTargetTeam.FRIENDLY]: 'союзный',
	[UnitTargetTeam.ENEMY]: 'вражеский',
	[UnitTargetTeam.ANY]: 'любой',
};

export class Item {
	constructor(data: Item) {
		Object.assign(this, data);
	}

	get typeTooltip() {
		return itemTypeTooltip[this.type];
	}

	get behaviorTooltip() {
		const result = this.behavior.map(it => behaviorTooltip[it]);
		return result.join(',');
	}

	get targetTooltip() {
		return `${targetTeamTooltip[this.targetTeam]} ${targetTypeTooltip[this.targetType]}`;
	}

	addModifier(modifier: Modifier) {
		this.modifiers.push(new Modifier(modifier));
	}
	removeModifier(id: string) {
		this.modifiers = this.modifiers.filter(it => it.id !== id);
	}
	is(state: ModifierState) {
		for (const modifier of this.modifiers) {
			if (modifier.state.includes(state)) {
				return true;
			}
		}
		return false;
	}

	id = '';
	name = '';
	description = '';
	lore = '';
	model = '';

	adding = true;
	removing = false;

	type = ItemType.DEFAULT;
	behavior = [ItemBehavior.NO_TARGET];
	targetTeam = UnitTargetTeam.NONE;
	targetType = UnitTargetType.NONE;
	ignoreTargetState: ModifierState[] = [];

	soundSet = {} as Record<ItemEvent, string>;

	modifiers: Modifier[] = [];
}
