<template>
  <div class="cart-sheet open">
    <!-- Header -->
    <div class="cart-header">
      <h3>Your Order</h3>
      <button class="close-btn" @click="$emit('close')">✕</button>
    </div>

    <!-- Items list -->
    <div class="cart-items-list">
      <div v-for="(group, key) in cartGrouped" :key="key" class="cart-row">
        <img :src="group.item.image || placeholder" class="cart-item-img" />
        <div class="cart-item-info">
          <div class="cart-item-name">{{ group.name }}</div>
          <div class="cart-item-price">
            Item Price: ${{ group.price.toFixed(2) }}
          </div>
          <div class="cart-item-qty">
            <button @click="$emit('change-qty', group.item, -1)">−</button>
            <span>{{ group.quantity }}</span>
            <button @click="$emit('change-qty', group.item, 1)">+</button>
          </div>
        </div>
        <div class="cart-item-subtotal">
          ${{ (group.quantity * group.price).toFixed(2) }}
        </div>
      </div>
    </div>

    <!-- Subtotal / VAT / Total -->
    <div class="cart-summary-details">
      <div class="summary-row">
        <span>Sub Total</span>
        <span>${{ total.toFixed(2) }}</span>
      </div>
      <div class="summary-row">
        <span>VAT (5%)</span>
        <span>${{ (total * 0.05).toFixed(2) }}</span>
      </div>
      <div class="summary-row total">
        <span>Total</span>
        <strong>${{ (total * 1.05).toFixed(2) }}</strong>
      </div>
    </div>

    <!-- Payment method + Pay button -->
    <div class="cart-pay-section">
      <select>
        <option>Cash</option>
        <option>Card</option>
      </select>
      <button class="pay-btn" @click="$emit('confirm')">
        Pay Now • ${{ (total * 1.05).toFixed(2) }}
      </button>
    </div>
  </div>
</template>

<script setup>
defineProps({
  cartGrouped: Object,
  total: Number
});

const placeholder = "/src/assets/placeholder.png";
</script>

<style scoped>
.cart-sheet {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  max-height: 80vh;
  background: #fff;
  border-radius: 16px 16px 0 0;
  box-shadow: 0 -6px 20px rgba(0, 0, 0, 0.25);
  padding: 16px;
  overflow-y: auto;
  z-index: 1000;
  transform: translateY(100%);
  transition: transform 0.3s ease;
}

.cart-sheet.open {
  transform: translateY(0);
}

.cart-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
}

.close-btn {
  background: none;
  border: none;
  font-size: 1.4rem;
  cursor: pointer;
}

.cart-items-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.cart-row {
  display: flex;
  gap: 12px;
  align-items: center;
}

.cart-item-img {
  width: 60px;
  height: 60px;
  object-fit: cover;
  border-radius: 8px;
}

.cart-item-info {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.cart-item-name {
  font-weight: 600;
}

.cart-item-price {
  font-size: 0.85rem;
  color: #666;
}

.cart-item-qty {
  display: flex;
  gap: 8px;
  margin-top: 4px;
}

.cart-item-qty button {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  border: 1px solid #ddd;
  background: #fff;
  font-size: 1rem;
}

.cart-item-subtotal {
  font-weight: 600;
}

.cart-summary-details {
  margin-top: 16px;
  border-top: 1px solid #eee;
  padding-top: 12px;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 4px;
  font-size: 0.95rem;
}

.summary-row.total {
  font-weight: bold;
  font-size: 1.1rem;
}

.cart-pay-section {
  display: flex;
  gap: 12px;
  margin-top: 16px;
}

.cart-pay-section select {
  flex: 1;
  padding: 10px;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.pay-btn {
  flex: 1;
  background: #ff8c00;
  border: none;
  color: #fff;
  font-weight: bold;
  padding: 12px;
  border-radius: 8px;
  cursor: pointer;
}
</style>